using AutoMapper;
using BlogAPI.Models;
using BlogAPI.Dtos;

namespace BlogAPI.Profiles
{
	public class ImageProfile : Profile
	{
		public ImageProfile()
		{
			CreateMap<ImageWriteDto, ImageEntity>();
			CreateMap<ImageEntity, ImageReadDto>()
			.ForMember(imageReadDto => imageReadDto.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(imageReadDto => imageReadDto.PostId, opt => opt.MapFrom(src => src.PostId))
			.ForMember(imageReadDto => imageReadDto.Uri, opt => opt.MapFrom(src => src.Uri))
			.IncludeAllDerived();
			
			// CreateMap<ICollection<ImageEntity>, ICollection<ImageReadDto>>();
		}
	}
}
