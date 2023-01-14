using System;
using Azure.Identity;
using Ajay.Legend.App.CSharp.Config;
using Ajay.Legend.App.Repositories;
using Ajay.Legend.App.Services;
using Defra.Cdp.HealthCheck;
using Defra.Cdp.Telemetry;
using Defra.Cdp.Telemetry.Config;
using Defra.Cdp.Telemetry.Loggers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;

namespace Ajay.Legend.App.CSharp
{
    /// <summary>
    /// Class to handle startup activities.
    /// NOTE - the new minimal startup in NET 6 only has a Program.cs (and not a Startup.cs) but
    /// here we use this class as a helper to Program.cs
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration settings
        /// </summary>
        private ConfigurationManager Configuration { get; }

        /// <summary>
        /// Environment properties
        /// </summary>
        private IWebHostEnvironment Environment { get; }

        /// <summary>
        /// App Insights telemetry client during startup.
        /// The standard logger cannot be used inside AddServices due to a chicken and egg situation of
        /// (a) the logger not yet being registered as a service
        /// (b) bad practice to call builder.Services.BuildServiceProvider()
        /// </summary>
        private StandardLogger startupLogger;

        /// <summary>
        /// Constructor for Startup
        /// </summary>
        /// <param name="configuration">config settings</param>
        /// <param name="environment">environment properties</param>
        public Startup(ConfigurationManager configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services">Service collection</param>
        public void AddServices(IServiceCollection services)
        {
            try
            {
                // Allow Potentially Identifiable Information (PII) to be included in exceptions,
                // since it will error if it detects any, and the sanitiser will remove PII anyway
                IdentityModelEventSource.ShowPII = true;

                services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#pragma warning disable IDE0058 // Expression value is never used
                //                // Handle requests behind Front Door/Load Balancer etc
                //                services.Configure<ForwardedHeadersOptions>(options =>
                //                {
                //                    options.ForwardedHeaders =
                //                        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                //                    options.KnownNetworks.Clear();
                //                    options.KnownProxies.Clear();
                //                });

                //
                // AppInsights registration
                //
                services.AddTelemetryServices(this.Configuration);

                // Note - AppInsights will not be fully registered as a service at this point
                // and getting a logger service by the use of builder.services.BuildServiceProvider()
                // is now an anti-pattern, so telemetryClient must be used to send logs to AppInsights.
                // Methods LogTrace() and LogException are available for sending logs during Startup.cs

                if (!this.EnvIsDevelopment())
                {
                    // Create Telemetry Client for logging to AppInsights during Startup.cs
                    this.startupLogger = new StandardLogger(this.Configuration, null);

                    //
                    // Link up to the Azure App Configuration Service
                    //
                    this.startupLogger.LogTrace($"Env is {this.Environment?.EnvironmentName}");
                    var appConfigEndPoint = this.Configuration.GetValue("AppConfig:Endpoint", "not found");
                    this.startupLogger.LogTrace($"AppConfig Endpoint = {appConfigEndPoint}");
                    var managedIdentityClientId = this.Configuration.GetValue("AppConfig:ManagedIdentityClientId", "not found");
                    this.startupLogger.LogTrace($"AppConfig managedIdentityClientId = {managedIdentityClientId}");

                    var credential = new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            ManagedIdentityClientId = managedIdentityClientId
                        }
                    );

                    // Set up App Config including downstream Key Vault, as well as auto-refresh of all values
                    // when the 'AutoReloadSentinel' value changes
                    this.Configuration.AddAzureAppConfiguration(options =>
                        options.Connect(new Uri(appConfigEndPoint), credential)
                               .ConfigureKeyVault(kv =>
                               {
                                   kv.SetCredential(credential);
                               })
                               .Select("*")
                               .ConfigureRefresh(refresh => refresh.Register("AutoReloadSentinel", refreshAll: true)
                    ));

                    this.startupLogger.LogTrace($"AppConfig registration completed");
                }

                //
                // Authentication
                //
                //services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                //   .AddMicrosoftIdentityWebApp(this.Configuration.GetSection("AzureAd"))
                //   .EnableTokenAcquisitionToCallDownstreamApi()
                //   .AddInMemoryTokenCaches();

                //if (!this.EnvIsDevelopment())
                //{
                //    services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
                //    {
                //        options.SaveTokens = true;
                //        options.Events = new OpenIdConnectEvents
                //        {
                //            OnRedirectToIdentityProvider = (context) =>
                //            {
                //                // Override the redirect_uri to https
                //                context.ProtocolMessage.RedirectUri = context.ProtocolMessage.RedirectUri.Replace("http://", "https://");
                //                return Task.FromResult(0);
                //            }
                //        };
                //    });
                //}

                //
                // Cookie policy
                //
                services.Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                    options.Secure = CookieSecurePolicy.Always;
                    // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1
                    options.HandleSameSiteCookieCompatibility();
                });

                //
                // Add APIs
                //
                var mvcBuilder = services.AddControllersWithViews(options =>
                {
                    //var policy = new AuthorizationPolicyBuilder()
                    //    .RequireAuthenticatedUser()
                    //    .Build();
                    //options.Filters.Add(new AuthorizeFilter(policy));
                });
                //.AddMicrosoftIdentityUI();

                services.AddRazorPages();

                if (this.EnvIsDevelopment())
                {
                    // Dynamic cshtml compilation for development environments
                    mvcBuilder.AddRazorRuntimeCompilation();
                }
                else
                {
                    // Use App Configuration Service when app is deployed
                    services.AddAzureAppConfiguration();
                }

                services.AddScoped<NonceConfig>();

                services.AddScoped(provider =>
                {
                    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                    var cookie = httpContextAccessor.HttpContext.Request.Cookies["CookieSettings"];
                    return cookie == null ? new CookieConfig() : JsonConvert.DeserializeObject<CookieConfig>(cookie);
                });

                services.AddHealthCheckServices();

                // Add services from the services project
                services.AddServiceTier(this.Configuration);

                // Add repositories (and DB registration) from the repositories project
                services.AddRepositoryTier(this.Configuration);
#pragma warning restore IDE0058 // Expression value is never used
            }
            catch (Exception ex)
            {
                // As AppInsights is not fully registered as a service at this point,
                // the LogTrace()/LogException() methods must be used here as they make calls
                // using a telemetry client.
                if (this.startupLogger != null)
                {
                    this.startupLogger.LogTrace($"Startup exception (see exceptions for full details): {ex.Message}");
                    this.startupLogger.LogException(ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Configure application
        /// </summary>
        /// <param name="app">Web application</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void Configure(WebApplication app)
        {
#pragma warning disable IDE0058 // Expression value is never used
            if (this.EnvIsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseForwardedHeaders();
            }
            else
            {
                app.UseExceptionHandler("/route-error");
                app.UseStatusCodePagesWithReExecute("/route-error/{0}");
                //app.UseForwardedHeaders();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseAzureAppConfiguration();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAppInsightsMiddlewareLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Index",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = HealthCheckExtensions.WriteResponse
                });
            });
#pragma warning restore IDE0058 // Expression value is never used
        }

        /// <summary>
        /// Determines if the current running environment is Development or not
        /// </summary>
        /// <returns>True if DEV or Development</returns>
        public bool EnvIsDevelopment()
        {
            return this.Environment?.EnvironmentName == null ||
                this.Environment.IsDevelopment() ||
                this.Environment.EnvironmentName == "DEV" ||
                this.Environment.EnvironmentName == "Development";
        }
    }
}
