using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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

		[HttpPost("user/{userId}/subscriptions/following/new"), Authorize, OwnerVerificationFilter]
		public async Task<ActionResult<SubscriptionReadDto?>> FollowAsync([FromRoute] int userId, [FromBody] SubscriptionWriteDto request, bool isOwner)
		{
			if (!isOwner || request.FromUserId != userId)
			{
				return Unauthorized();
			}

			if (request.FromUserId == request.ToUserId)
			{
				return BadRequest("Cannot subscribe to self!");
			}

			var newSub = await _subscriptionRepository.AddAsync(_mapper.Map<SubscriptionEntity>(request));
			return Ok(_mapper.Map<SubscriptionReadDto>(newSub));
		}

		[HttpDelete("user/{userId}/subscriptions/following/{userToUnfollowId}"), Authorize, OwnerVerificationFilter]
		public async Task<ActionResult<SubscriptionReadDto?>> UnfollowAsync([FromRoute] int userId, [FromRoute] int userToUnfollowId, bool isOwner)
		{
			if (!isOwner)
			{
				return Unauthorized();
			}

			var user = await _userRepository.GetAsync(userId);
			var sub = user.Following.FirstOrDefault(subscription => subscription.FromUser.Id == userId && subscription.ToUserId == userToUnfollowId);
			if (sub is null)
			{
				return NotFound("Subscription not found!");
			}
			var deletedSub = await _subscriptionRepository.DeleteAsync(sub.Id);
			
			return Ok(_mapper.Map<SubscriptionReadDto>(deletedSub));
		}

		[HttpGet("user/{userId}/subscriptions/following"), Authorize]
		public async Task<ActionResult<List<SubscriptionReadDto?>>> GetFollowingAsync([FromRoute] int userId)
		{
			var user = await _userRepository.GetAsync(userId);
			return user.Following.Select(subscription => _mapper.Map<SubscriptionReadDto?>(subscription)).ToList(); 
		}
		
		[HttpGet("user/{userId}/subscriptions/followers"), Authorize]
		public async Task<ActionResult<List<SubscriptionReadDto?>>> GetFollowersAsync([FromRoute] int userId)
		{
			var user = await _userRepository.GetAsync(userId);
			return user.Followers.Select(subscription => _mapper.Map<SubscriptionReadDto?>(subscription)).ToList(); 
		}
	}	
}