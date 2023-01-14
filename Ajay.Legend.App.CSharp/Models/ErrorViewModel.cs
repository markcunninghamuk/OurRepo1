namespace Ajay.Legend.App.CSharp.Models
{
    /// <summary>
    /// Model to hold error information.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets a flag to denote if the request id should be displayed (if it exists).
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

        /// <summary>
        /// Gets or sets the contact email address.
        /// </summary>
        public string ContactEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the contact description e.g. team name.
        /// </summary>
        public string ContactDescription { get; set; }
    }
}