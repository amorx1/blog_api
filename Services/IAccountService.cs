using System;
using BlogAPI.Models;
using BlogAPI.PostgreSQL;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Services
{
	public interface IAccountService
	{
		//UserEntity GetUser(int id);
		int GetUserId();
		string GetUsername();
		string GetEmailAddress();
		void Blacklist();
        bool MatchesId(int requestUserId);
        Task<bool> TokenIsValid(int requestUserId);
		Task<bool> ResolveUser(int requestUserId) => Task.FromResult(false);
    }
}

