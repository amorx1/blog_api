using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Dtos;
using BlogAPI.AttributeFiters;
using BlogAPI.Repositories;
using AutoMapper;

namespace BlogAPI.Controllers
{
	[ApiController]
	[Route("api")]
	public class SubscriptionsController : BaseController<SubscriptionEntity, SubscriptionReadDto, SubscriptionWriteDto, EFSubscriptionRepository>
	{
		private readonly EFSubscriptionRepository _subscriptionRepository;
		private readonly EFUserRepository _userRepository;
		private readonly IMapper _mapper;
		
		public SubscriptionsController(EFSubscriptionRepository subscriptionRepository, EFUserRepository userRepository, IMapper mapper) : base(subscriptionRepository, mapper)
		{
			_subscriptionRepository = subscriptionRepository;
			_userRepository = userRepository;
			_mapper = mapper;
		}

		[HttpPost("user/{userId}/subscriptions/follow"), OwnerVerificationFilter]
		public async Task<ActionResult<SubscriptionReadDto?>> SubscribeAsync([FromRoute] int userId, [FromBody] SubscriptionWriteDto request, bool isOwner)
		{
			if (!isOwner || request.FromUserId != userId)
			{
				return Unauthorized();
			}

			var newSub = await _subscriptionRepository.AddAsync(_mapper.Map<SubscriptionEntity>(request));
			return Ok(_mapper.Map<SubscriptionReadDto>(newSub));
		}

		[HttpGet("user/{userId}/subscriptions/following"), OwnerVerificationFilter]
		public async Task<ActionResult<List<SubscriptionReadDto?>>> GetFollowingAsync([FromRoute] int userId, bool isOwner)
		{
			if (!isOwner)
			{
				return Unauthorized();
			}

			var user = await _userRepository.GetAsync(userId);
			return user.Following.Select(f => _mapper.Map<SubscriptionReadDto>(f)).ToList(); 
		}
	}	
}