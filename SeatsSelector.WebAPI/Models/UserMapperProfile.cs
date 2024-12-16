using AutoMapper;
using SeatsSelector.Shared.Models.Users;
using SeatsSelector.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatsSelector.Shared.Models
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile() {

            CreateMap<CreateUser, UserEntity>();

            CreateMap<UserEntity, User>();
        }
    }
}
