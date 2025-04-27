using Microsoft.AspNetCore.Mvc.Rendering;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IUserService
{
    public Task<(List<UserListViewModel> , int totalpage)> UserListAsync(string sortBy , bool desc , int page , int pagesize , string serachcriteria);

    public Task<List<UserListViewModel>> AddDataToUserListViewModel(List<Profile> user_list);

    public Task<List<UserListViewModel>> SortUserList(List<UserListViewModel> user_list_data , string sortBy , bool desc);

    public Task<List<UserListViewModel>> SearchUserList(List<UserListViewModel> user_list_data , string searchcriteria);

    public Task<List<UserListViewModel>> GetPaginationList(List<UserListViewModel> user_list_data , int page , int pagesize);

    public Task<bool> ChangePasswordAsync(ChangePasswordViewModel obj);

    public Task<bool> UserDeleteAsync(int userId);

    public Task<(SelectList countries , SelectList roles)> GetAddUserAsync();

    public Task<List<State>> GetStatesAsync(int Countryid);

    public Task<List<City>> GetCitiesAsync(int Stateid);

    public Task<bool> AddUserAsync(AddUserViewModel obj);

    public Task<EditUserViewModel> GetEditUserAsync(int userid);

    public Task<(SelectList states , SelectList cities)> GetSelectedStatesCities(int countryId , int statesId);

    public Task<bool> EditUserAsync(EditUserViewModel obj);

    public Task<ProfileViewModel> GetProfileViewModelAsync();

    public Task<bool> ProfileViewModelAsync(ProfileViewModel obj);
}
