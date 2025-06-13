namespace Immotech.Front.Models;

public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
    public bool IsMain { get; set; }
} 