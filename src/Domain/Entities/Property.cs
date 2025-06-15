namespace Domain.Entities;

public class Property
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public required Address Address { get; set; }
    public required string Location { get; set; }
    public decimal Price { get; set; }
    public PropertyStatus Status { get; set; }
    public int Bedrooms { get; set; }
    public int SurfaceArea { get; set; }
    public PropertyType Type { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public int? AgencyId { get; set; }
    public Guid? UserId { get; set; }

    // Navigation properties
    public Agency? Agency { get; set; }
    public ICollection<Photo> Photos { get; set; } = [];

    

    

}
public enum PropertyStatus
    {
        Available,
        Rented,
        Sold,
        Pending
    }

public enum PropertyType
    {
        House,
        Apartment,
        Land,
        Garage,
        Office,
        Shop,
        Warehouse,
        ApartmentBuilding,
        Hotel,
        Other
    }