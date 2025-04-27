using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class UserLoginService : IUserLoginService
{
    private readonly IUserLoginRepository _userLoginRepository;
    private readonly IJWTService _jwtService;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpaccessor;

    public UserLoginService(IUserLoginRepository userLoginRepository , IJWTService jwtService, IEmailService emailService , IHttpContextAccessor httpaccessor){
        _userLoginRepository = userLoginRepository;
        _jwtService = jwtService;
        _emailService = emailService;
        _httpaccessor = httpaccessor;
    }

    public async Task ForgotPasswordAsync(string email)
    {
        var userExists = await _userLoginRepository.GetUserByEmailAsync(email);
        if(userExists == null){
            throw new Exception("User with email does'nt exists.");
        }
        string token = Guid.NewGuid().ToString();
        await _userLoginRepository.AddResetPasswordData(email , token);
        string message = $@"
                    <HTML>
                        <body>
                            <p>To reset the password click on <a href='http://localhost:5096/UserLogin/ResetPassword?email={email}&token={token}'>Reset Password</a>
                        </body>
                    </HTML>
                ";
        await _emailService.SendAsyncEmail(email , "ResetPassword" , message);
    }


    public async Task<(string Token , int UserId , string role)> LoginAsync(UserLoginViewModel obj)
    {
        var existingUser = await _userLoginRepository.GetUserByEmailAsync(obj.Email);
        if(existingUser == null){
            throw new Exception("User with email does'nt exists.");
        }
        if(!BCrypt.Net.BCrypt.Verify(obj.Passwordhashed , existingUser.UserLogins.First().Passwordhashed)){
            throw new Exception("Incorrect password");
        }
        var token = _jwtService.GenerateJwtToken(existingUser.UserLogins.First().Email , existingUser.UserLogins.First().Passwordhashed , existingUser.UserRoles.First().Role.Roles);
        var userid = existingUser.UserLogins.First().Userid;
        _httpaccessor.HttpContext.Session.SetString("ProfileImg" , existingUser.Profileimg);
        _httpaccessor.HttpContext.Session.SetString("UserName" , existingUser.Username);
        var role = existingUser.UserRoles.First().Role.Roles;
        return (token , userid , role);
    }

    public async Task<(bool check , int UserId , string role)> RememberMe(string token)
    {
        var email = _jwtService.GetClaimValue(token , "email");
        var role = _jwtService.GetClaimValue(token , "role");
        var pass = _jwtService.GetClaimValue(token , "pass");
        var checkUser = await _userLoginRepository.GetUserByEmailAsync(email);
        if(pass == checkUser.UserLogins.First().Passwordhashed){
            var userid = checkUser.UserLogins.First().Userid;
            _httpaccessor.HttpContext.Session.SetString("ProfileImg" , checkUser.Profileimg);
            _httpaccessor.HttpContext.Session.SetString("UserName" , checkUser.Username);
            return (true , userid , role);
        }
        return (false , 0 , role);
    }

    public async Task<ResetPasswordViewModel> ResetPasswordAsync(string email, string token)
    {
        ResetPasswordViewModel obj = new ResetPasswordViewModel();
        var user = await _userLoginRepository.GetResetpasswordUserAsync(email , token);
        if(user != null){
            obj.Email = email;
        }
        return obj;
    }

    public async Task<bool> ResetPasswordUpdate(ResetPasswordViewModel obj)
    {
        if(obj.Password == obj.ConfirmePassword){
            var userExists = await _userLoginRepository.GetUserByEmailAsync(obj.Email);
            if(userExists != null){
                var passwordhashed = BCrypt.Net.BCrypt.HashPassword(obj.Password);
                await _userLoginRepository.UpdateUserLoginPassword(userExists , passwordhashed);
                return true;
            }
        }
        return false;
    }

}
