using TeaStoreApi.Models;

namespace TeaStoreApi.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> RegisterUser(User user);
        Task<User> LoginUser(string email, string password);
    }
}
