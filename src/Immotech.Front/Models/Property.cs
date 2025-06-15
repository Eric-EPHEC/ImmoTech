namespace Immotech.Front.Models;

public class Property
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Address Address { get; set; }
    public required string Location { get; set; }
    public decimal Price { get; set; }
    public PropertyStatus Status { get; set; }
    public int Bedrooms { get; set; }
    public int SurfaceArea { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public int? AgencyId { get; set; }
    public Agency? Agency { get; set; }
    public List<Photo>? Photos { get; set; }
    public PropertyType Type { get; set; }

} 