using DbApi;

namespace MessageApi.Abstractions
{
    public interface IMessageService
    {
        public void SendMessage(string recipient, string text, UserModel currentuser);
        public List<MessageModel> GetMessage(UserModel recipient);
        public UserModel GetCurrentUser(HttpContext httpContext);
        public List<MessageModel> GetTestMessage(UserModel recipient); // для теста
    }
}
