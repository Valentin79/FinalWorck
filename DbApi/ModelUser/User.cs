

namespace DbApi
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public UserRole RoleId { get; set; }
        public virtual Role Role { get; set; }
        //public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    } 

}