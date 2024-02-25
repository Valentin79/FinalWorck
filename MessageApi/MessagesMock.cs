using DbApi;

namespace MessageApi
{
    public class MessagesMock : IMessageTestService
    {
        public UserModel GetTestUser()
        {
            var user = new UserModel{
                Id = 2,
                Name = "user2",
                Email = "user2@mail.ru",
                Password = "password",
                Role = UserRole.User
            };
            return user;
        }

        public List<Message> TestMessages()
        {
            var messages = new List<Message>();
            messages.Add(new Message
            {
                MessageId = 1,
                FromUserId = 1, 
                ToUserId = 2,
                IsRead = false,
                Text = "это тестовое сообщение"
            });
            messages.Add(new Message
            {
                MessageId = 2,
                FromUserId = 1,
                ToUserId = 2,
                IsRead = false,
                Text = "это еще одно тестовое сообщение"
            });

            return messages;
        }
    }
}
