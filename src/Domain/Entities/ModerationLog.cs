namespace Domain.Entities
{
    public class ModerationLog
    {
        public int Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid ModeratorId { get; set; }
        public required string Action { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public Property? Property { get; set; }
        public User? Moderator { get; set; }
    }
}