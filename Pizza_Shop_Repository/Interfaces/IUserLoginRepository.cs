using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface IUserLoginRepository
{
    public Task<Profile> GetUserByEmailAsync(string email);

    public Task AddResetPasswordData(string email , string token);

    public Task<Resetpassword> GetResetpasswordUserAsync(string email , string token);

    public Task UpdateUserLoginPassword(Profile obj , string HashPassword);
}
