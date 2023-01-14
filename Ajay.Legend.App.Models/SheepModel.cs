using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ajay.Legend.App.Models;
public class SheepModel : AuditableModel
{
    public SheepModel()
    {
        this.VisualAttributes = new List<string>();
    }

    /// <summary>
    /// Gets or sets the Id.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("First name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("Last name")]
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the colour type.
    /// </summary>
    [Required]
    [DisplayName("Colour type")]
    public int? ColourType { get; set; }

    /// <summary>
    /// Gets or sets the visual attributes.
    /// </summary>
    [DisplayName("Visual attributes")]
    public IEnumerable<string>? VisualAttributes { get; set; }

    /// <summary>
    /// Gets or sets the image url.
    /// </summary>
    [DisplayName("Photo url")]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the medical history.
    /// </summary>
    [DisplayName("Medical history")]
    public string? MedicalHistory { get; set; }

    /// <summary>
    /// Gets a list of options used to populate the radio group.
    /// </summary>
    public static IEnumerable<SelectListItem> ColourTypeOptions
    {
        get =>
            new List<SelectListItem> {
                    new SelectListItem { Text = "White", Value = "1" },
                    new SelectListItem { Text = "Black", Value = "2" },
                    new SelectListItem { Text = "Mixed", Value = "3" },
        };
    }

    /// <summary>
    /// Gets a list of options used to populate the checkbox group.
    /// </summary>
    public static IEnumerable<SelectListItem> VisualAttributesOptions
    {
        get =>
            new List<SelectListItem> {
                    new SelectListItem { Text = "Some white on face", Value = "some-white-face" },
                    new SelectListItem { Text = "Some black on face", Value = "some-black-face" },
                    new SelectListItem { Text = "Branding mark", Value = "branding-mark" },
                    new SelectListItem { Text = "Long-haired", Value = "long-haired" },
        };
    }
}
