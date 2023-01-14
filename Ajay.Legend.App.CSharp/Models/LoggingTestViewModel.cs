using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ajay.Legend.App.CSharp.Models
{
    /// <summary>
    /// View model to define form elements for logging test page.
    /// </summary>
    public class LoggingTestViewModel
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the standard message.
        /// </summary>
        [Required]
        [DisplayName("Standard log message text")]
        public string StandardMessage { get; set; }

        /// <summary>
        /// Gets or sets the standard logger viewer url.
        /// </summary>
        public string StandardLoggerViewerUrl { get; set; }

        /// <summary>
        /// Gets or sets the exception text.
        /// </summary>
        [Required]
        [DisplayName("Custom exception text")]
        public string ExceptionText { get; set; }

        /// <summary>
        /// Gets or sets the standard logger viewer url.
        /// </summary>
        public string ExceptionLoggerViewerUrl { get; set; }

        /// <summary>
        /// Gets or sets the debug message.
        /// </summary>
        [Required]
        [DisplayName("Debug message text")]
        public string DebugMessage { get; set; }

        /// <summary>
        /// Gets or sets the standard logger viewer url.
        /// </summary>
        public string DebugLoggerViewerUrl { get; set; }
    }
}
