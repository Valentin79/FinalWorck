using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DbApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserApi.Abstractions;


namespace UserApi.Controller
{
    public class RsaTools
    {
        // как на лекции было, так и сделал =)
        public static RSA GetPrivateKey()
        {
            var f = File.ReadAllText("rsa/private_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(f);
            return rsa;
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class UserLoginController: ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IUserAuthenticationService _userAuthenticationService; // только для тестирования

        public UserLoginController(IUserService userService, IConfiguration configuration,
            IUserAuthenticationService userAuthenticationService)
        {
            _userService = userService;
            _configuration = configuration;
            _userAuthenticationService = userAuthenticationService; // только для тестирования
        }

        // Только для тестирования
        [AllowAnonymous]
        [HttpPost]
        [Route("LoginTest")]
        public ActionResult TestLogin([FromBody] LoginModel userLogin)
        {
            var user = _userAuthenticationService.Authenticate(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            return NotFound("пользователь не найден");
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] LoginModel userLogin)
        {
            try
            {
                var roleId = _userService.UserCheck(userLogin.Email, userLogin.Password);
                var user = new UserModel { Email = userLogin.Email, Role = roleId };
                var token = GenerateToken(user);
                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private string GenerateToken(UserModel user)
        {
            var key = new RsaSecurityKey(RsaTools.GetPrivateKey());
            var cradentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claim,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: cradentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
