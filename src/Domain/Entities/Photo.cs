namespace Domain.Entities;

public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
}