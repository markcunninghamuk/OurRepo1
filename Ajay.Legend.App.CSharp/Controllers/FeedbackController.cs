using Microsoft.AspNetCore.Mvc;

namespace Ajay.Legend.App.CSharp.Controllers
{
    /// <summary>
    /// FeedbackController.
    /// </summary>
    [Route("")]
    public class FeedbackController : Controller
    {
        /// <summary>
        /// Feedback.
        /// </summary>
        /// <returns>Feedback view.</returns>
        [Route("feedback")]
        [HttpGet]
        public IActionResult Feedback()
        {
            this.ViewBag.Title = "Feedback";
            return this.View();
        }
    }
}
