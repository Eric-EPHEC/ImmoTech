namespace Immotech.Front.Models;

public class PropertyListResponse
{
    public List<Property> Properties { get; set; } = [];
    public int TotalCount { get; set; }
} 