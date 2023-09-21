using AutoMapper;
using BlogAPI.Dtos;
using BlogAPI.Models;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Profiles
{
	public class PostProfile : Profile
	{
		public PostProfile()
		{	
			CreateMap<PostEntity, PostReadDto>()
			.ForMember(postReadDto => postReadDto.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
			.ForMember(postReadDto => postReadDto.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(postReadDto => postReadDto.Title, opt => opt.MapFrom(src => src.Title))
			.ForMember(postReadDto => postReadDto.Body, opt => opt.MapFrom(src => src.Body))
			.ForMember(postReadDto => postReadDto.Images, opt => opt.MapFrom(src => src.Images))
			.IncludeAllDerived();

			// CreateMap<List<PostEntity>, List<PostReadDto>>();

			CreateMap<PostWriteDto, PostEntity>()
			.ForMember(postEntity => postEntity.Title, opt => opt.MapFrom(src => src.Title))
			.ForMember(postEntity => postEntity.Body, opt => opt.MapFrom(src => src.Body))
			.ForMember(postEntity => postEntity.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
			.ForMember(postEntity => postEntity.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
			.ForMember(postEntity => postEntity.Images, opt => opt.MapFrom(src => src.Images));
		}
	}
}