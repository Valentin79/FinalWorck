using DbApi;

namespace MessageApi
{
    public interface IMessageTestService
    {
        public List<Message> TestMessages();
        UserModel GetTestUser();
    }
}
