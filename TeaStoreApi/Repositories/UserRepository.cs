using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TeaStoreApi.Data;
using TeaStoreApi.Interfaces;
using TeaStoreApi.Models;

namespace TeaStoreApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApiDbContext dbContext;
        public UserRepository(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<User> LoginUser(string email, string password)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null && user.Password != HashPassword(password))    
            {
                return null;
            }
            return user;
        }

        public async Task<bool> RegisterUser(User user)
        {
            var userExists = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userExists != null)
            {
                return false;
            }

            user.Password = HashPassword(user.Password);

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return true;
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

    }
}
