using DbApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Abstractions;


namespace UserApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UserControllers : ControllerBase
         {
         private readonly IUserService _userService;  

         public UserControllers(IUserService userService, IConfiguration configuration)
         {
             _userService = userService;
         }   



            [AllowAnonymous]
            [HttpPost]
            [Route("AddUser")]
            public ActionResult AddUser([FromBody] LoginModel userLogin, string name)
            {
                try
                {
                    _userService.AddUser(name, userLogin.Email, userLogin.Password);
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
                return Ok();
            }


            
            [HttpPost]
            [Route("AddAdmin")]
            [Authorize(Roles = "Administrator")]
            public ActionResult AddAdmin([FromBody] LoginModel userLogin, string name)
            {
                try
                {
                    _userService.AddAdmin(name, userLogin.Email, userLogin.Password);
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
                return Ok();
            }


            
            [HttpDelete]
            [Route("DeleteUser")]
            [Authorize(Roles = "Administrator")]
            public ActionResult UserDelete(int id)
            {
                try
                {
                    _userService.DeleteUser(id);
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
                return Ok();
            }


            [Authorize]
            [HttpGet]
            [Route("GetUsers")]
            public ActionResult<List<UserModel>> GetUsers()
            {
                try
                {
                    var result = _userService.GetUsers();
                    return result;
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }


            [Authorize]
            [HttpGet]
            [Route("GetUser")]
            public ActionResult<UserModel> GetUser(int? id, string email)
            {
                try
                {
                    var result = _userService.GetUser(id, email);
                    return result;
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
    }


}
