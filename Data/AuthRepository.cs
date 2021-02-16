using System.Threading.Tasks;
using DotnetRpg.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        
        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var result = new ServiceResponse<int>() {Data = user.Id};
            if (await UserExists(user.Username))
            {
                result.Success = false;
                result.Message = "User already exists";

                return result;
            }
            
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            
            return result;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}