namespace Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset SentAt { get; set; }
    public bool IsRead { get; set; }

    // SenderEmail is required because a notification can be sent to an agency or a user
    public required string SenderEmail { get; set; }

    // RecipientEmail is required because a notification can be sent to an agency or a user
    public required string RecipientEmail { get; set; }

    public Guid? RecipientId { get; set; } // UserId of the recipient
    public User? Recipient { get; set; } // User who received the notification

    public int? AgencyId { get; set; } // AgencyId of the agency
    public Agency? Agency { get; set; } // Agency who sent the notification
}