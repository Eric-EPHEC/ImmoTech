namespace Domain.Entities;

public class Agency
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Address Address { get; set; }
    public required string ContactEmail { get; set; }

    public ICollection<Property> Properties { get; set; } = [];
    public ICollection<ProfessionalUser> ProfessionalUsers { get; set; } = [];
}