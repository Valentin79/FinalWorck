

namespace DbApi
{
    public partial class Role
    {
        public UserRole RoleId { get; set; }
        public string Email { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
