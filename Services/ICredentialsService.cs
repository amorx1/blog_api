using BlogAPI.PostgreSQL;

namespace BlogAPI.Services
{
    public interface ICredentialsService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(UserEntity user);
    }
}