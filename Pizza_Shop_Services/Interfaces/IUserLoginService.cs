using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IUserLoginService
{
    public Task<(string Token , int UserId , string role)> LoginAsync(UserLoginViewModel obj);

    public Task<(bool check , int UserId, string role)> RememberMe(string token);

    public Task ForgotPasswordAsync(string email);

    public Task<ResetPasswordViewModel> ResetPasswordAsync(string email , string token);

    public Task<bool> ResetPasswordUpdate(ResetPasswordViewModel obj);
}
