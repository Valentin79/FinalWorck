
namespace DbApi
{
    public class Message
    {
        public int MessageId { get; set; }
        public int FromUserId { get; set; }
        public virtual User? FromUser { get; set; }
        public int ToUserId { get; set; }
        public virtual User? ToUser { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Text { get; set; }
    }
}
