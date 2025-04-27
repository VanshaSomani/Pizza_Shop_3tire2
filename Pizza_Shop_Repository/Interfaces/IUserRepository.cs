using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface IUserRepository
{
    public Task<List<Profile>> GetUserList();

    public Task<Profile> UpdatePassword(String pass , Profile user);

    public Task<bool> DeleteUserData(int UserId , int updateUserId);

    public Task<List<Country>> GetCountries();

    public Task<List<Role>> GetRoles();

    public Task<List<State>> GetState(int Countryid);

    public Task<List<City>> GetCity(int Stateid);

    public Task<bool> AddUser(Profile profile , UserLogin userLogin , UserRole userRole);

    public Task<Profile> GetUserByUserId(int userid);

    public Task<Profile> AddDataToEditUserViewModel(int userid);

    public Task<bool> EditUser(Profile profile , UserRole userRole);

    public Task<Profile> AddDataToProfileViewModel(int userid);

    public Task<bool> ChangeProfile(Profile obj);

}
