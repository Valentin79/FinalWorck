
namespace DbApi
{
    public class MessageModel
    {
        public int MessageId { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public bool IsRead { get; set; }
        public DateTime Date { get; set; } 
        public string Text { get; set; }
    }
}
