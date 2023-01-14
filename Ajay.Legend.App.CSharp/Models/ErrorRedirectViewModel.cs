using System;

namespace Ajay.Legend.App.CSharp.Models
{
    /// <summary>
    /// ErrorRedirectViewModel.
    /// </summary>
    public class ErrorRedirectViewModel : ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the url that the user attempted to navigate to.
        /// </summary>
        public Uri RedirectUri { get; set; }
    }
}
