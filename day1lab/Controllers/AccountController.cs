using day1lab.DTO;
using day1lab.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace day1lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }
        //Registeration
        [HttpPost("register")] //api/account/register
        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email
                };
                IdentityResult result =await userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return Ok("Account Added Successfuly");
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")] //api/account/login
        public async Task<IActionResult> Login(LoginUserDTO loginDTO)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(loginDTO.UserName);
                if (user != null && await userManager.CheckPasswordAsync(user, loginDTO.Password))
                {
                    // Token Claims
                    List<Claim> userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id     ));
                    userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    //get roles
                    var roles = await userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                    SigningCredentials sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    //create Token
                    JwtSecurityToken myToken = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],//web api url
                    audience: configuration["JWT:ValidAudience  "], //consumer angular url
                    expires: DateTime.Now.AddDays(1),
                    claims: userClaims,
                    signingCredentials: sc
                    );

                    return Ok(new
                                {
                                    token = new JwtSecurityTokenHandler().WriteToken(myToken),
                                    expire = myToken.ValidTo
                                }
                             );











                    return Ok("Login Successful");
                }
                return Unauthorized("Can't find this user");

            }

            return Unauthorized("Invalid Authentication");
        }















    }
}
