using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class UserRepository : IUserRepository
{
    private readonly RmsdemoContext _db;
    public UserRepository(RmsdemoContext db){
        _db = db;
    }

    #region AddDataToEditUserViewModel
    public async Task<Profile> AddDataToEditUserViewModel(int userid)
    {
        Profile check = await GetUserByUserId(userid);
        return check;
    }  
    #endregion

    #region AddUser
        public async Task<bool> AddUser(Profile profile , UserLogin userLogin , UserRole userRole)
        {
            try
            {
                _db.Profiles.Add(profile);
                _db.SaveChanges();
                
                userLogin.Userid = profile.Userid;
                userLogin.Email = profile.Email;
                _db.UserLogins.Add(userLogin);
                _db.SaveChanges();

                userRole.Userid = profile.Userid;
                _db.UserRoles.Add(userRole);
                _db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region EditUser
        public async Task<bool> EditUser(Profile profile , UserRole userRole){
            try
            {            
                _db.Profiles.Update(profile);
                await _db.SaveChangesAsync();

                _db.UserRoles.Update(userRole);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region ChangeProfile
        public async Task<bool> ChangeProfile(Profile obj){
            try
            {
                _db.Profiles.Update(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region DeleteUserData
        public async Task<bool> DeleteUserData(int UserId , int updateUserId)
        {
            try
            {    
                Profile user = await _db.Profiles.FirstOrDefaultAsync(u => u.Userid == UserId);
                user.Isdeleted = true;
                user.Updatedby = updateUserId;
                user.Updatedat = DateTime.Now;
                _db.Profiles.Update(user);
                await _db.SaveChangesAsync();

                UserLogin user_login = _db.UserLogins.FirstOrDefault(u => u.Userid == UserId);
                user_login.Isdeleted = true;
                _db.UserLogins.Update(user_login);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region GetCity
        public async Task<List<City>> GetCity(int Stateid)
        {
            return _db.Cities.Where(c => c.Stateid == Stateid).ToList();
        }
    #endregion

    #region GetCountries
        public async Task<List<Country>> GetCountries()
        {
            return await _db.Countries.ToListAsync();
        }
    #endregion

    #region GetRoles
        public async Task<List<Role>> GetRoles()
        {
            return await _db.Roles.ToListAsync();
        }
    #endregion

    #region GetState
        public async Task<List<State>> GetState(int Countryid)
        {
            return _db.States.Where(s => s.Countryid == Countryid).ToList();
        }
    #endregion

    #region GetUserByUserId
        public async Task<Profile> GetUserByUserId(int userid)
        {
            return await _db.Profiles.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Userid == userid);
        }
    #endregion

    #region GetUserList
        public async Task<List<Profile>> GetUserList()
        {
            List<Profile> user_list = _db.Profiles.Include(u => u.UserRoles).ThenInclude(u => u.Role).Where(u => u.Isdeleted == false).ToList();
            return user_list;
        }
    #endregion

    #region UpdatePassword
        public async Task<Profile> UpdatePassword(String pass , Profile user)
        {
            user.UserLogins.First().Passwordhashed = pass;
            user.UserLogins.First().Updatedby = user.Userid;
            user.UserLogins.First().Updatedat = DateTime.Now;
            _db.UserLogins.Update(user.UserLogins.First());
            await _db.SaveChangesAsync();
            return user;
        }
    #endregion

    #region AddDataToProfileViewModel
        public async Task<Profile> AddDataToProfileViewModel(int userid){
            Profile check = await _db.Profiles.Include(c=>c.Country).Include(s=>s.State).Include(ci=>ci.City).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(u => u.Userid == userid);
            return check;
        }
    #endregion

}
