using AutoMapper;
using BlogAPI.Dtos;
using BlogAPI.Models;

namespace BlogAPI.Profiles
{
	public class PostProfile : Profile
	{
		public PostProfile()
		{
			CreateMap<PostEntity, PostReadDto>()
			.ForMember(postReadDto => postReadDto.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
			.ForMember(postReadDto => postReadDto.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(postReadDto => postReadDto.Title, opt => opt.MapFrom(src => src.Title))
			.ForMember(postReadDto => postReadDto.Body, opt => opt.MapFrom(src => src.Body));
		}
	}
}
