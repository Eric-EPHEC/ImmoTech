using System.ComponentModel.DataAnnotations;

namespace Immotech.Front.Models;

// DTO used in Create / Edit forms
public class PropertyUpsertModel
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    // Address fields
    [Required] public string Street { get; set; } = string.Empty;
    [Required] public string City { get; set; } = string.Empty;
    [Required] public string State { get; set; } = string.Empty;
    [Required] public string ZipCode { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Range(0, 1_000_000_000)] // in euros
    public int Price { get; set; } // in euros

    [Range(0, 50)]
    public int Bedrooms { get; set; } // number of bedrooms

    [Range(0, 100000)]
    public int SurfaceArea { get; set; } // in m²

    [Required]
    public PropertyType Type { get; set; } = PropertyType.House; // default value

    [Required]
    public PropertyBidType BidType { get; set; } = PropertyBidType.Sale; // default

    public int? AgencyId { get; set; }

    [Required]
    public PropertyStatus Status { get; set; } = PropertyStatus.Available; // default
} 