using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeatsSelector.Shared.Models.Rooms;
using SeatsSelector.Shared.Models.Seats;
using SeatsSelector.WebAPI.Database;
using SeatsSelector.WebAPI.Models;
using System;

namespace SeatsSelector.WebAPI.Controllers
{
    [ApiController]
    [Route("seats")]
    public class SeatsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;



        // --------------------------------------------------------

        public SeatsController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }



        // --------------------------------------------------------

        [HttpGet]
        public async Task<ActionResult<List<Seat>>> GetAll(int? roomId = null)
        {
            IQueryable<SeatEntity> query = _dbContext.Seats;

            if (roomId.HasValue)
                query = query.Where(seat => seat.RoomId == roomId);


            var Seats = await query.ToListAsync();

            return Seats.Select(room => _mapper.Map<Seat>(room)).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Seat>> Post(CreateSeat createSeat)
        {
            var seatEntity = _mapper.Map<SeatEntity>(createSeat);

            _dbContext.Seats.Add(seatEntity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Seat>(seatEntity);
        }

        [HttpPost("assign-user")]
        public async Task<ActionResult<Seat>> Post(AssignUser assignUser)
        {
            SeatEntity seatEntity = _dbContext.Seats.Include(seat => seat.User).FirstOrDefault(seat => seat.Id == assignUser.SeatId);

            if (seatEntity == null)
                return BadRequest("This seat doesn't exist. Please try again.");

            if (seatEntity.UserId != null)
                return BadRequest($"This seat is already taken by {seatEntity.User.DisplayName}.");

            UserEntity userEntity = _dbContext.Users.Include(user => user.Seat).FirstOrDefault(user => user.Id == assignUser.UserId);

            if (userEntity == null)
                return BadRequest("This user doesn't exist. Please try again.");

            if (userEntity.Seat != null)
            {
                userEntity.Seat.UserId = null;

                _dbContext.Seats.Update(userEntity.Seat);
                await _dbContext.SaveChangesAsync();
            }

            seatEntity.UserId = userEntity.Id;

            _dbContext.Seats.Update(seatEntity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Seat>(seatEntity);
        }

        [HttpPost("batch")]
        public async Task<ActionResult<List<Seat>>> BatchPost(List<CreateSeat> createSeats)
        {
            var seatEntity = createSeats.Select(createSeat => _mapper.Map<SeatEntity>(createSeat)).ToList();

            _dbContext.Seats.AddRange(seatEntity);
            await _dbContext.SaveChangesAsync();

            return createSeats.Select(seatEntity => _mapper.Map<Seat>(seatEntity)).ToList();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var seatEntity = _dbContext.Seats.FirstOrDefault(seat => seat.Id == id);

            if (seatEntity == null)
                return NotFound();

            _dbContext.Seats.Remove(seatEntity);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
