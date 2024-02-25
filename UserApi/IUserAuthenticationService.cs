using DbApi;

namespace UserApi
{
    public interface IUserAuthenticationService
    {
        UserModel Authenticate(LoginModel model);
    }
}
