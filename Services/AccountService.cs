using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlogAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace BlogAPI.Services
{
	public class AccountService : IAccountService
	{
		private readonly HttpContext? _httpContext;
		private readonly IConnectionMultiplexer _redisCache;
		private readonly IDatabase _redisDb;

		public AccountService(IHttpContextAccessor httpContextAccessor, IConnectionMultiplexer redis)
		{
			_httpContext = httpContextAccessor.HttpContext;
			_redisCache = redis;
			_redisDb = _redisCache.GetDatabase();
		}

		public string GetUserId()
		{
			var idString = _httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return idString;
        }

        public string GetUsername()
		{
			var username = _httpContext?.User.FindFirstValue(ClaimTypes.Name);
			return username != null ? username : throw new Exception("Error fetching identity");
		}
		public string GetEmailAddress()
		{
            var email = _httpContext?.User.FindFirstValue(ClaimTypes.Email);
            return email != null ? email : throw new Exception("Error fetching identity");
        }
		public async void Blacklist()
		{
			var stream = _httpContext?.Request.Headers["Authorization"].ToString().Split()[1];
			var handler = new JwtSecurityTokenHandler();
			var token = handler.ReadToken(stream);
			var deleteFromRedisAt = token.ValidTo;
			var diff = deleteFromRedisAt - DateTime.Now;

			await _redisDb.StringSetAsync(stream, 0, diff);

			return;
		}
		public async Task<bool> TokenIsValid(int userRequestId)
		{
			if (!MatchesId(userRequestId))
			{
				return false;
			}

			// get bearer token from header
			var token = _httpContext?.Request.Headers["Authorization"].ToString().Split()[1];
			// check if token is in blacklisted tokens (for accounts recently deleted)
			return !await _redisDb.KeyExistsAsync(token);
		}

        public bool MatchesId(int requestUserId)
		{
			var tokenId = GetUserId();
			return tokenId != null && Int32.Parse(tokenId) == requestUserId;
		}

		public async Task<bool> ResolveUser(int requestUserId)
		{
			return await TokenIsValid(requestUserId) && MatchesId(requestUserId);
		}
	}
}
