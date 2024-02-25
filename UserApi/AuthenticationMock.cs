using DbApi;

namespace UserApi
{
    public class AuthenticationMock : IUserAuthenticationService
    {
        public UserModel Authenticate(LoginModel model)
        {
            if (model.Email == "admin@mail.ru" && model.Password == "password")
            {
                return new UserModel { 
                    Email = model.Email, 
                    Password = model.Password, 
                    Role = UserRole.Administrator };
            }
            if (model.Email == "user@mail.ru" && model.Password == "super")
            {
                return new UserModel
                {
                    Email = model.Email,
                    Password = model.Password,
                    Role = UserRole.User
                };
            }
            return null;
        }
    }
}
