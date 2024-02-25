using DbApi;


namespace UserApi.Abstractions
{
    public interface IUserService
    {
        public void AddUser(string name, string email, string password);

        public void AddAdmin(string name, string email, string password);

        public void DeleteUser(int id);

        public UserModel GetUser(int? id, string? email);

        public List<UserModel> GetUsers();

        public UserRole UserCheck(string email, string password);
    }
}
