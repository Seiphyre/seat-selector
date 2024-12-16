using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeatsSelector.Shared.Models.Rooms;
using SeatsSelector.WebAPI.Database;
using SeatsSelector.WebAPI.Models;
using System;

namespace SeatsSelector.WebAPI.Controllers
{
    [ApiController]
    [Route("rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;



        // --------------------------------------------------------

        public RoomsController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }



        // --------------------------------------------------------

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAll()
        {
            var rooms = await _dbContext.Rooms.Include(r => r.Seats).ToListAsync();

            return rooms.Select(room => _mapper.Map<Room>(room)).ToList();
        }

        [HttpGet("{roomId}")]
        public async Task<ActionResult<Room>> Get(int roomId)
        {
            var roomEntity = await _dbContext.Rooms.Include(r => r.Seats).FirstOrDefaultAsync(room => room.Id == roomId);

            if (roomEntity == null)
                return NotFound();

            return _mapper.Map<Room>(roomEntity);
        }

        [HttpPost]
        public async Task<ActionResult<Room>> Post(CreateRoom createRoom)
        {
            var roomEntity = _mapper.Map<RoomEntity>(createRoom);

            _dbContext.Rooms.Add(roomEntity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Room>(roomEntity);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var roomEntity = _dbContext.Rooms.FirstOrDefault(room => room.Id == id);

            if (roomEntity == null)
                return NotFound();

            _dbContext.Rooms.Remove(roomEntity);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
