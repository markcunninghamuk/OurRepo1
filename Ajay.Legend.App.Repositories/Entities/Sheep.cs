using System;
using System.ComponentModel.DataAnnotations;

namespace Ajay.Legend.App.Repositories.Entities
{
    /// <summary>
    /// Sheep.
    /// </summary>
    public class Sheep : AuditableEntity
    {
        public Sheep(Guid id, string? firstName, string? lastName)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the colour type.
        /// </summary>
        public int ColourType { get; set; }

        /// <summary>
        /// Gets or sets the visual attributes.
        /// </summary>
        public string? VisualAttributesAsCsv { get; set; }

        /// <summary>
        /// Gets or sets the image url.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the medical history.
        /// </summary>
        public string? MedicalHistory { get; set; }
    }
}

