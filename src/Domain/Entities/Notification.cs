namespace Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset SentAt { get; set; }
    public bool IsRead { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}