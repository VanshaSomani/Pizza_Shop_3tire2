using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class UserLoginRepository : IUserLoginRepository
{
    private readonly RmsdemoContext _db;

    public UserLoginRepository(RmsdemoContext db){
        _db = db;
    }

    public async Task AddResetPasswordData(string email, string token)
    {
        Resetpassword userReset = new Resetpassword();
        userReset.Email = email;
        userReset.Resettoken = token;
        userReset.Resettokenvalidity = DateTime.UtcNow.AddHours(24);
        await _db.AddAsync(userReset);
        await _db.SaveChangesAsync();
    }

    public async Task<Resetpassword> GetResetpasswordUserAsync(string email, string token)
    {
        Resetpassword userReset = await _db.Resetpasswords.FirstOrDefaultAsync(u => u.Email == email && u.Resettoken == token && u.Resettokenvalidity > DateTime.UtcNow);
        return userReset;
    }

    public async Task<Profile> GetUserByEmailAsync(string email)
    {
        Profile foundUser =  await _db.Profiles.Include(u => u.UserLogins).Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Email == email && u.Isdeleted == false);
        // Console.WriteLine(foundUser);
        return foundUser;
    }

    public async Task UpdateUserLoginPassword(Profile obj, string HashPassword)
    {
        obj.UserLogins.First().Passwordhashed = HashPassword;
        _db.SaveChangesAsync();
    }

}
