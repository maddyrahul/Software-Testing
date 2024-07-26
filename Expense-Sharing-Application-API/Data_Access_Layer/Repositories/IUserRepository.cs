using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByEmailAndPasswordAsync(string email, string password);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<bool> UserExistsAsync(string email);

        Task<User> GetUserByEmail(string email);

    }
}
