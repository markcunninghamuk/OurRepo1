using Ajay.Legend.App.CSharp.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Net;

namespace Ajay.Legend.App.CSharp.Controllers
{
    /// <summary>
    /// ErrorController.
    /// </summary>
    [Route("")]
    public class ErrorController : Controller
    {
        private readonly string contactDescription;
        private readonly string contactEmailAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration from appsettings/AppConfigurationService.</param>
        public ErrorController(IConfiguration configuration)
        {
            this.contactEmailAddress = configuration.GetValue("Errors:ContactEmailAddress", string.Empty);
            this.contactDescription = configuration.GetValue("Errors:ContactDescription", string.Empty);
        }

        /// <summary>
        /// Error.
        /// </summary>
        /// <param name="statusCode">The statusCode.</param>
        /// <returns>Error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("route-error")]
        [Route("route-error/{statusCode}")]
        public IActionResult Route(HttpStatusCode? statusCode)
        {
            var exceptionHandlerPathFeature = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var statusCodeReExecuteFeature = this.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var originalPath = exceptionHandlerPathFeature?.Path
                ?? (statusCodeReExecuteFeature == null ? null
                : statusCodeReExecuteFeature.OriginalPath + statusCodeReExecuteFeature.OriginalQueryString);

            return statusCode == HttpStatusCode.Forbidden
                ? this.RedirectToAction(nameof(this.AccessDenied), new { redirectUri = originalPath })
                : statusCode == HttpStatusCode.NotFound
                ? this.RedirectToAction(nameof(this.NotFound), new { redirectUri = originalPath })
                : (IActionResult)this.RedirectToAction(nameof(this.GenericError));
        }

        /// <summary>
        /// Access denied.
        /// </summary>
        /// <param name="redirectUri">The url that the user attempted to navigate to.</param>
        /// <returns>Access denied view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("access-denied")]
        public IActionResult AccessDenied(Uri redirectUri)
        {
            this.ViewBag.Title = "Access denied";
            var vm = this.BuildVm(redirectUri);
            return this.View(vm);
        }

        /// <summary>
        /// Not found.
        /// </summary>
        /// <param name="redirectUri">The url that the user attempted to navigate to.</param>
        /// <returns>Access denied view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("page-not-found")]
        public IActionResult NotFound(Uri redirectUri)
        {
            this.ViewBag.Title = "Page not found";
            var vm = this.BuildVm(redirectUri);
            return this.View(vm);
        }

        /// <summary>
        /// Error.
        /// </summary>
        /// <returns>Error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("error")]
        public IActionResult GenericError()
        {
            this.ViewBag.Title = "Error";
            var vm = this.BuildVm();
            return this.View(vm);
        }

        private ErrorRedirectViewModel BuildVm(Uri redirectUri = null)
        {
            return new ErrorRedirectViewModel
            {
                RedirectUri = redirectUri,
                ContactDescription = this.contactDescription,
                ContactEmailAddress = this.contactEmailAddress,
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
            };
        }
    }
}
