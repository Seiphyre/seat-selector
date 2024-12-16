using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using SeatsSelector.Shared.Models.Users;
using SeatsSelector.WebAPI.Database;
using SeatsSelector.WebAPI.Models;
using SeatsSelector.Shared.Models;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;



namespace SeatsSelector.WebAPI.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;



        // --------------------------------------------------------

        public AccountController(ApplicationDbContext dbContext, IMapper mapper, UserManager<UserEntity> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }



        // --------------------------------------------------------

        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate(Authenticate authenticate)
        {
            UserEntity userEntity = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == authenticate.UserName);

            if (userEntity == null)
                return BadRequest();

            // --

            bool passwordMatch = await _userManager.CheckPasswordAsync(userEntity, authenticate.Password);

            if (!passwordMatch)
                return BadRequest();

            // --

            User user = _mapper.Map<User>(userEntity);

            // --

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            // Retrieve the JWT secret from environment variables and encode it
            var key = Encoding.ASCII.GetBytes("rzN7BmKiBZVJu/ymJcbUSs3xdUAY4wRkcJasW+EH0Fc=");

            // Create an identity from the claims
            ClaimsIdentity identity = (ClaimsIdentity)user.ToClaimsPrincipal().Identity;

            // Describe the token settings
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "seat-selector-webapi",
                Audience = "seat-selector-app",
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create a JWT security token
            var token = jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            // Write the token as a string and return it
            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
