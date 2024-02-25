using DbApi;
using MessageApi.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;

namespace MessageApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController: ControllerBase
    {

        private readonly IMessageService _messageService;
        private readonly IMessageTestService _messageTestService; // только для теста

        public MessageController(IMessageService messageService, IMessageTestService messageTestService)
        {
            _messageService = messageService;
            _messageTestService = messageTestService; // только для теста
        }


        [HttpPost]
        [Route("SendMessage")]
        [Authorize]
        public ActionResult SendMessage(string recipient, string text)
        {
            try
            {
                var currentuser = _messageService.GetCurrentUser(HttpContext);
                _messageService.SendMessage(recipient, text, currentuser);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            return Ok();

        }

        [HttpGet]
        [Route("GetMessages")]
        [Authorize]
        public ActionResult<List<MessageModel>> GetMessages()
        {
            try
            {
                var currentuser = _messageService.GetCurrentUser(HttpContext);
                var messages = _messageService.GetMessage(currentuser);
                return messages;
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
        }

        // Для теста
        [HttpGet]
        [Route("GetTestMessages")]
        [AllowAnonymous]
        public ActionResult<List<MessageModel>> GetTestMessages()
        {
            var currentuser = _messageTestService.GetTestUser();
            var messages = _messageService.GetTestMessage(currentuser);
            return messages;
        }
    }
}
