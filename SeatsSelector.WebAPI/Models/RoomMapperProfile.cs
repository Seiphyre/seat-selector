using AutoMapper;
using SeatsSelector.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatsSelector.Shared.Models.Rooms
{
    public class RoomMapperProfile : Profile
    {
        public RoomMapperProfile() {

            CreateMap<CreateRoom, RoomEntity>();

            CreateMap<RoomEntity, Room>();
        }
    }
}
