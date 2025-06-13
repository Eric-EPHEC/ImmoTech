namespace Immotech.Front.Models;

public class Agency
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Address Address { get; set; }
    public required string ContactEmail { get; set; }
} 