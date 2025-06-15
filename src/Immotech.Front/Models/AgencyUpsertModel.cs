using System.ComponentModel.DataAnnotations;

namespace Immotech.Front.Models;

public class AgencyUpsertModel
{
    public int? Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }

    // Address fields
    [Required] public string Street { get; set; } = string.Empty;
    [Required] public string City { get; set; } = string.Empty;
    [Required] public string State { get; set; } = string.Empty;
    [Required] public string ZipCode { get; set; } = string.Empty;
} 