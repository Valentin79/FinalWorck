using System.Security.Claims;
using AutoMapper;
using DbApi;
using MessageApi.Abstractions;
using Microsoft.AspNetCore.Http;
using UserApi.DB;
using static System.Net.Mime.MediaTypeNames;

namespace MessageApi.Service
{
    public class MessageService : IMessageService
    {
        private IMapper _mapper;
        private AppDbContext _appDbContext;
        private readonly IMessageTestService _messageTestService; // только для теста

        public MessageService(IMapper mapper, AppDbContext appDbContext, IMessageTestService messageTestService)
        {
            this._mapper = mapper;
            this._appDbContext = appDbContext;
            this._messageTestService = messageTestService;
        }

        public List<MessageModel> GetTestMessage(UserModel currentuser) // только для теста
        {
            var messageList = new List<MessageModel>();
            var messages = _messageTestService.TestMessages();

                foreach (var message in messages)
                {
                    
                    message.IsRead = true;
                    messageList.Add(_mapper.Map<MessageModel>(message));
                }

                return messageList;
        }
        

        public List<MessageModel> GetMessage(UserModel currentuser)
        {
            var messageList = new List<MessageModel>();
            using (_appDbContext)
            {

                var messages = _appDbContext.Messages.Where
                    (x => x.ToUserId == currentuser.Id && !x.IsRead).ToList();
                
                foreach (var message in messages)
                {
                    message.IsRead = true;
                    messageList.Add(_mapper.Map<MessageModel>(message));
                }

                _appDbContext.SaveChanges();

                return messageList;
            }
        }

        public void SendMessage(string recipient, string text, UserModel currentuser)
        {
            Console.WriteLine($"{currentuser.Id}, {currentuser.Email}, {currentuser.Name}");
            using (_appDbContext)
            {
                var userTo = _appDbContext.Users.FirstOrDefault(x => x.Email == recipient);
                if (userTo == null)
                {
                    throw new Exception("адрессат не найден");
                }

                var message = new Message();
                message.FromUserId = currentuser.Id;
                message.ToUserId = userTo.Id;
                message.Text = text;

                _appDbContext.Messages.Add(message);
                _appDbContext.SaveChanges();
            }
        }

        public UserModel GetCurrentUser(HttpContext httpcontext)
        {
            var identity = httpcontext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;

                string? email = userClaims.FirstOrDefault
                    (x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                var user = _mapper.Map<UserModel>
                    (_appDbContext.Users.FirstOrDefault(x => x.Email == email));

                return user;
            }
            return null;
        }
    }
}
