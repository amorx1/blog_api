
using System.Data.Entity.Core.Common.CommandTrees;
using AutoMapper;
using BlogAPI.Dtos;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserReadDto>();
            CreateMap<UserWriteDto, UserEntity>();
        }    
    }
}
