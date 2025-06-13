namespace Immotech.Front.Models;

public class Agency
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ContactEmail { get; set; }
    public string? LogoUrl { get; set; }
} 