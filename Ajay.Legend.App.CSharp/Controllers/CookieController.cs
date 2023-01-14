using Ajay.Legend.App.CSharp.Models;
using Defra.Cdp.Telemetry.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using Newtonsoft.Json;
using System;
using System.Web;

namespace Ajay.Legend.App.CSharp.Controllers
{
    /// <summary>
    /// CookieController.
    /// </summary>
    [AllowAnonymous]
    [Route("Cookie")]
    public class CookieController : Controller
    {
        private readonly CookieConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieController"/> class.
        /// </summary>
        /// <param name="config">The cookie config.</param>
        public CookieController(CookieConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Cookie policy.
        /// </summary>
        /// <returns>Cookie policy view.</returns>
        [Route("cookie-policy")]
        [HttpGet]
        public IActionResult CookiePolicy()
        {
            this.ViewBag.Title = "Cookie policy";
            return this.View();
        }

        /// <summary>
        /// Cookie settings.
        /// </summary>
        /// <returns>Cookie settings view.</returns>
        [Route("cookie-settings")]
        [HttpGet]
        public IActionResult CookieSettings()
        {
            this.ViewBag.Title = "Cookie settings";
            var vm = new CookieSettingsViewModel
            {
                Config = this.config,
            };

            return this.View(vm);
        }

        /// <summary>
        /// Cookie settings.
        /// </summary>
        /// <param name="vm">The view model.</param>
        /// <returns>Cookie settings view.</returns>
        [Route("cookie-settings")]
        [HttpPost]
        public IActionResult CookieSettings(CookieSettingsViewModel vm)
        {
            if (vm != null)
            {
                this.SetCookieSettings(vm.Config);
            }
            return this.RedirectToAction("cookie-settings");
        }

        /// <summary>
        /// Cookie settings.
        /// </summary>
        /// <param name="cookies">Selection made by user (accept or reject).</param>
        /// <param name="redirectUri">The uri to redirect to.</param>
        /// <returns>Cookie settings view.</returns>
        [HttpPost]
        public IActionResult SetCookies(string cookies, Uri redirectUri)
        {
            if (!string.IsNullOrEmpty(cookies))
            {
                this.SetCookieSettings(cookies == "accept" ? CookieConfig.AllAllowed : CookieConfig.RejectAdditional);
            }

            if (redirectUri != null)
            {
                var uri = QueryHelpers.AddQueryString(redirectUri.ToString(), "cookie-confirmation", "show");
                return this.LocalRedirect(uri);
            }
            return this.View();
        }

        /// <summary>
        /// Cookie settings.
        /// </summary>
        /// <param name="redirectUri">The uri to redirect to.</param>
        /// <returns>Cookie settings view.</returns>
        [Route("cookie-settings/hide-confirmation")]
        [HttpPost]
        public IActionResult HideCookieConfirmation(Uri redirectUri)
        {
            if (redirectUri != null)
            {

                if (!redirectUri.IsAbsoluteUri)
                {
                    redirectUri = new Uri($"{this.Request.Scheme}://{this.Request.Host}{redirectUri}");
                }

                var query = HttpUtility.ParseQueryString(redirectUri.Query);
                query.Remove("cookie-confirmation");
                return this.LocalRedirect(redirectUri.PathAndQuery);
            }
            return this.View();
        }

        private void SetCookieSettings(CookieConfig config)
        {
            config.IsSet = true;
            if (!config.AllowWebsiteUseCookies)
            {
                this.Response.Cookies.Delete("ai_session");
                this.Response.Cookies.Delete("ai_user");
            }

            var cookie = JsonConvert.SerializeObject(config);
            var cookieOptions = new CookieOptions
            {
                Secure = false,
                Expires = DateTime.UtcNow.AddYears(1),
                IsEssential = true,
            };
            this.Response.Cookies.Append("CookieSettings", cookie, cookieOptions);
        }
    }
}
