using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using SeatsSelector.Shared.Models.Users;
using SeatsSelector.WebAPI.Database;
using SeatsSelector.WebAPI.Models;



namespace SeatsSelector.WebAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;



        // --------------------------------------------------------

        public UsersController(ApplicationDbContext dbContext, IMapper mapper, UserManager<UserEntity> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }



        // --------------------------------------------------------

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _dbContext.Users.ToListAsync();

            return users.Select(userEntity => _mapper.Map<User>(userEntity)).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(CreateUser createUser)
        {
            var userEntity = _mapper.Map<UserEntity>(createUser);

            var result = await _userManager.CreateAsync(userEntity, createUser.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return _mapper.Map<User>(userEntity);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userEntity = _dbContext.Users.FirstOrDefault(seat => seat.Id == id);

            if (userEntity == null)
                return NotFound();

            _dbContext.Users.Remove(userEntity);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
