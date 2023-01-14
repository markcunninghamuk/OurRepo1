using System.Collections.Generic;
using Ajay.Legend.App.CSharp.Models;
using Defra.Cdp.Telemetry.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Ajay.Legend.App.CSharp.Controllers
{
    /// <summary>
    /// A controller to handle the logging test page.
    /// </summary>
    [AllowAnonymous]
    public class LoggingTestController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IStandardLogger standardLogger;
        private readonly IFullExceptionLogger fullExceptionLogger;
        private readonly IDebugLogger debugLogger;

        /// <summary>
        /// COnstructor for LoggingTestController.
        /// </summary>
        /// <param name="configuration">The application config (from appsettings or App Config service)</param>
        /// <param name="standardLogger"></param>
        /// <param name="fullExceptionLogger"></param>
        /// <param name="debugLogger"></param>
        public LoggingTestController(IConfiguration configuration, IStandardLogger standardLogger, IFullExceptionLogger fullExceptionLogger, IDebugLogger debugLogger)
        {
            this.standardLogger = standardLogger;
            this.fullExceptionLogger = fullExceptionLogger;
            this.debugLogger = debugLogger;
            this.configuration = configuration;
        }

        /// <summary>
        /// Displays the logging test page.
        /// </summary>
        /// <returns>Action result.</returns>
        public IActionResult LoggingTest()
        {
            var vm = new LoggingTestViewModel
            {
                StandardLoggerViewerUrl = this.configuration.GetValue("ApplicationInsights:StandardLoggerViewerUrl", "#"),
                ExceptionLoggerViewerUrl = this.configuration.GetValue("ApplicationInsights:FullExceptionLoggerViewerUrl", "#"),
                DebugLoggerViewerUrl = this.configuration.GetValue("ApplicationInsights:DebugLoggerViewerUrl", "#"),
            };

            return this.View(vm);
        }

        /// <summary>
        /// Handles the posted values from the logging test page.
        /// </summary>
        /// <param name="model">View model posted from the page.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public IActionResult LoggingTest(LoggingTestViewModel model)
        {
            // Remove errors not related to current sub-form
            if (model.Action == "standard")
            {
                this.KeepErrorsOnlyForElement("StandardMessage");
                if (this.ElementHasError("StandardMessage"))
                {
                    return this.View(model);
                }
                this.standardLogger.LogTrace(model.StandardMessage);
                return this.RedirectToAction("LoggingTest");
            }
            else if (model.Action == "exception")
            {
                this.KeepErrorsOnlyForElement("ExceptionText");
                if (this.ElementHasError("ExceptionText"))
                {
                    return this.View(model);
                }
                this.fullExceptionLogger.LogException(new System.ArgumentException(model.ExceptionText));
                return this.RedirectToAction("LoggingTest");
            }
            else if (model.Action == "debug")
            {
                this.KeepErrorsOnlyForElement("DebugMessage");
                if (this.ElementHasError("DebugMessage"))
                {
                    return this.View(model);
                }
                this.debugLogger.Debug(model.DebugMessage);
                return this.RedirectToAction("LoggingTest");
            }

            return this.View(model);
        }

        private void KeepErrorsOnlyForElement(string elementName)
        {
            var elementNames = new List<string> { "StandardMessage", "ExceptionText", "DebugMessage" };
            _ = elementNames.Remove(elementName);
            for (var i = 0; i < elementNames.Count; i++)
            {
                if (this.ModelState.ContainsKey(elementNames[i]))
                {
                    this.ModelState[elementNames[i]].Errors.Clear();
                }
            }
        }

        private bool ElementHasError(string elementName)
        {
            return this.ModelState.ContainsKey(elementName) && this.ModelState[elementName].Errors.Count > 0;
        }
    }
}
