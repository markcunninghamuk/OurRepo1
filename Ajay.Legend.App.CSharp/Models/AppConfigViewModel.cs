using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ajay.Legend.App.CSharp.Models
{
    /// <summary>
    /// View model to define form elements.
    /// </summary>
    public class AppConfigViewModel
    {
        /// <summary>
        /// Gets or sets the config item name.
        /// </summary>
        [DisplayName("Config item name")]
        [Required]
        public string ConfigName { get; set; }

        /// <summary>
        /// Gets or sets the config item value.
        /// </summary>
        [DisplayName("Retrieved value")]
        public string ConfigValue { get; set; }

        /// <summary>
        /// List of config key names.
        /// </summary>
        public IEnumerable<SelectListItem> ConfigKeys { get; set; }

        /// <summary>
        /// Environment name.
        /// </summary>
        public string EnvironmentName { get; set; }

    }
}