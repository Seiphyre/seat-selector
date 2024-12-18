using AutoMapper;
using SeatsSelector.Shared.Models.Seats;
using SeatsSelector.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatsSelector.Shared.Models
{
    public class SeatMapperProfile : Profile
    {
        public SeatMapperProfile() {

            CreateMap<CreateSeat, SeatEntity>();

            CreateMap<SeatEntity, Seat>()
                .ForMember(dest => dest.IsOccupied, opt => opt.MapFrom(src => src.UserId != null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>  $"{(char)('A' + src.Row)}-{src.Col + 1}"));
        }
    }
}
