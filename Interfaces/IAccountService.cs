namespace BlogAPI.Interfaces
{
	public interface IAccountService
	{
		//UserEntity GetUser(int id);
		string GetUserId();
		string GetUsername();
		string GetEmailAddress();
		void Blacklist();
        bool MatchesId(int requestUserId);
        Task<bool> TokenIsValid(int requestUserId);
		Task<bool> ResolveUser(int requestUserId) => Task.FromResult(false);
    }
}

