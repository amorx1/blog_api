using AutoMapper;
using BlogAPI.Models;
using BlogAPI.Dtos;

namespace BlogAPI.Profiles
{
	public class SubscriptionProfile : Profile
	{
		public SubscriptionProfile()
		{
			CreateMap<SubscriptionWriteDto, SubscriptionEntity>();
			CreateMap<SubscriptionEntity, SubscriptionReadDto>()
			.IncludeAllDerived();		
		}
	}
}
