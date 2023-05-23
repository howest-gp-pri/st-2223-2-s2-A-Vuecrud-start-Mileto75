using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pri.Ca.Api.DTOs.Account;
using Pri.Ca.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pri.Ca.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AccountLoginRequestDto accountLoginRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }
            //check credentials
            var user = await _userManager.FindByNameAsync(accountLoginRequestDto.Username);
            if(!await _userManager.CheckPasswordAsync(user,accountLoginRequestDto.Password))
            {
                return BadRequest("Wrong credentials");
            }
            //generate token
            //set parameters
            var audience = _configuration.GetValue<string>("JWTConfiguration:Audience");
            var issuer = _configuration.GetValue<string>("JWTConfiguration:Issuer");
            var expires = _configuration.GetValue<int>("JWTConfiguration:Expires");
            var secret = _configuration.GetValue<string>("JWTConfiguration:Secret");
            //claims
            var claims = await _userManager.GetClaimsAsync(user);
            //signinKey
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signinCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            //generate byte token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience:audience,
                claims:claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(expires),
                signingCredentials: signinCredentials
                );
            //serialize token
            var serializedToken = new JwtSecurityTokenHandler()
                .WriteToken(token);
            return Ok(serializedToken);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(AccountRegisterRequestDto accountRegisterRequestDto)
        {
            var user = await _userManager.FindByNameAsync(accountRegisterRequestDto.Username);
            List<IdentityResult> identityResults = new();
            if(user == null)
            {
                user = new ApplicationUser 
                {
                    Firstname = accountRegisterRequestDto.Firstname,
                    Lastname = accountRegisterRequestDto.Lastname,
                    UserName = accountRegisterRequestDto.Username,
                    Email = accountRegisterRequestDto.Username,
                    DateOfBirth = accountRegisterRequestDto.DateOfBirth,
                };
                identityResults.Add(await _userManager.CreateAsync(user, accountRegisterRequestDto.Password));
                if(identityResults.All(ie => ie.Succeeded))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role,"user"),
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.DateOfBirth,user.DateOfBirth.ToShortDateString()),
                    };
                    identityResults.Add(await _userManager.AddClaimsAsync(user, claims));
                }
                if(identityResults.Any(ie => ie.Succeeded == false))
                {
                    return BadRequest("Something went wrong, please try again later...");
                }
                return Ok();
            }
            return BadRequest("Username taken");
        }
    }
}
