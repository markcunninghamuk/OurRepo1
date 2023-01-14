using Microsoft.AspNetCore.Mvc;
using Ajay.Legend.App.CSharp.Models;
using Defra.Cdp.Telemetry.Loggers;

namespace Ajay.Legend.App.CSharp.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private readonly IStandardLogger logger;

        /// <summary>
        /// Constructor for Home controller.
        /// </summary>
        /// <param name="logger">Logger for AppInsights</param>
        public HomeController(IStandardLogger logger)
        {
            this.logger = logger;
            var LogMessage = "Initiating the Home Controller for the Hello World PoC";
            this.logger.LogTrace(LogMessage);
        }

        /// <summary>
        /// Displays the home page.
        /// </summary>
        /// <returns>Action result.</returns>
        public IActionResult Index()
        {
            var model = new IndexModel();
            // Write Business Logic Here!
            return this.View(model);
        }
    }
}