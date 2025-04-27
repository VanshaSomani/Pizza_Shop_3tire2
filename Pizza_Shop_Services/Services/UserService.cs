using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;
namespace Pizza_Shop_Services.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJWTService _jwtService;
    private readonly IUserLoginRepository _userLoginRepository;
    private readonly IHttpContextAccessor _httpaccessor;
    private readonly IWebHostEnvironment _env;
    private readonly IEmailService _emailService;

    #region UserService    
        public UserService(IUserRepository userRepository , IJWTService jwtService , IUserLoginRepository userLoginRepository , IHttpContextAccessor httpaccessor ,IWebHostEnvironment env , IEmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _userLoginRepository = userLoginRepository;
            _httpaccessor = httpaccessor;
            _env = env;
            _emailService = emailService;
        }
    #endregion

    #region ChangePasswordAsync    
        public async Task<bool> ChangePasswordAsync(ChangePasswordViewModel obj)
        {
            var token = _httpaccessor.HttpContext.Request.Cookies["jwtToken"];
            var email = _jwtService.GetClaimValue(token , "email");
            Profile user = await _userLoginRepository.GetUserByEmailAsync(email);
            bool verified = BCrypt.Net.BCrypt.Verify(obj.OldPassword , user.UserLogins.First().Passwordhashed);
            if(obj.NewPassword == obj.ConfirmePassword && verified == true){
                string password_Hash = BCrypt.Net.BCrypt.HashPassword(obj.NewPassword);
                var updateduser = await _userRepository.UpdatePassword(password_Hash , user);
                var jwtToken = _jwtService.GenerateJwtToken(updateduser.Email , updateduser.UserLogins.First().Passwordhashed , updateduser.UserRoles.First().Role.Roles);
                _httpaccessor.HttpContext.Response.Cookies.Append("jwtToken", jwtToken);
                return true;
            }
            return false;
        }
    #endregion

    #region UserDeleteAsync    
        public async Task<bool> UserDeleteAsync(int userId)
        {
            try
            {
                int UpdateuserId = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                bool status = await _userRepository.DeleteUserData(userId , UpdateuserId);
                return status;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region UserListAsync
        public async Task<(List<UserListViewModel> , int totalpage)> UserListAsync(string sortBy , bool desc , int page , int pagesize , string searchcriteria)
        {
            List<Profile> user_list = await _userRepository.GetUserList();
            List<UserListViewModel> user_list_data = await AddDataToUserListViewModel(user_list);
            List<UserListViewModel> sorted_user_list = await SortUserList(user_list_data , sortBy , desc);
            List<UserListViewModel> serach_User_List = await SearchUserList(sorted_user_list,searchcriteria);
            List<UserListViewModel> reduced_user_list = await GetPaginationList(serach_User_List , page , pagesize);
            return (reduced_user_list , serach_User_List.Count());
        }
    #endregion

    #region AddDataToUserListViewModel
        public async Task<List<UserListViewModel>> AddDataToUserListViewModel(List<Profile> user_list)
        {
            List<UserListViewModel> user_list_data = new List<UserListViewModel>();
            foreach(Profile user in user_list){
                UserListViewModel user_obj = new UserListViewModel();
                user_obj.Userid = user.Userid;
                user_obj.Firstname = user.Firstname;
                user_obj.Lastname = user.Lastname;
                user_obj.Email = user.Email;
                user_obj.Phoneno = user.Phoneno;
                user_obj.profileimg = user.Profileimg;
                UserRole userRole = user.UserRoles.FirstOrDefault();
                if(userRole != null){
                    user_obj.Rolename = userRole.Role.Roles;
                }
                user_obj.status = (bool)user.Status;
                user_list_data.Add(user_obj);
            }
            return user_list_data;
        }        
    #endregion

    #region SortUserList
        public Task<List<UserListViewModel>> SortUserList(List<UserListViewModel> user_list_data , string sortBy , bool desc){
            List<UserListViewModel> sorted_list = sortBy switch
            {
                "Name" => desc ? user_list_data.OrderByDescending(u => u.Firstname).ToList() : user_list_data.OrderBy(u => u.Firstname).ToList(),
                "Role" => desc ? user_list_data.OrderByDescending(u => u.Rolename).ToList() : user_list_data.OrderBy(u => u.Rolename).ToList(),
                _ => user_list_data
            };
            return Task.FromResult(sorted_list);
        }
    #endregion

    #region SearchUserList
        public async Task<List<UserListViewModel>> SearchUserList(List<UserListViewModel> user_list_data , string searchcriteria)
        {
            if(!string.IsNullOrEmpty(searchcriteria)){
                List<UserListViewModel> serach_User_List = user_list_data.Where(u => u.Firstname.Contains(searchcriteria , StringComparison.OrdinalIgnoreCase) ||
                    (u.Lastname!=null && u.Lastname.Contains(searchcriteria , StringComparison.OrdinalIgnoreCase)) ||
                    (u.Rolename!=null && u.Rolename.Contains(searchcriteria , StringComparison.OrdinalIgnoreCase))).ToList();
                return serach_User_List;
            }
            return user_list_data;
        }
    #endregion

    #region GetPaginationList
        public async Task<List<UserListViewModel>> GetPaginationList(List<UserListViewModel> user_list_data, int page, int pagesize)
        {
            List<UserListViewModel> reduced_user_list =  user_list_data.Skip((page - 1)*pagesize).Take(pagesize).ToList();
            return reduced_user_list;
        }
    #endregion

    #region GetAddUserAsync
        public async Task<(SelectList countries , SelectList roles)> GetAddUserAsync(){
            return (new SelectList(await _userRepository.GetCountries() , "Countryid", "CountryName") , new SelectList(await _userRepository.GetRoles() , "Roleid" , "Roles"));
        }
    #endregion

    #region GetStates
        public Task<List<State>> GetStatesAsync(int Countryid){
            return _userRepository.GetState(Countryid);
        }
    #endregion

    #region GetCities
        public Task<List<City>> GetCitiesAsync(int Stateid)
        {
            return _userRepository.GetCity(Stateid);
        }
    #endregion

    #region AddUserAsync
        public async Task<bool> AddUserAsync(AddUserViewModel obj)
        {
            try
            {
                string filename = "";
                string filepath = "";
                if(obj.profileimg != null){
                    string folder = Path.Combine(_env.WebRootPath , "ProfileImages");
                    filename = Guid.NewGuid().ToString() + "_" + obj.profileimg.FileName;
                    filepath = Path.Combine(folder , filename);
                    obj.profileimg.CopyTo(new FileStream(filepath , FileMode.Create));
                }
                var password_Hash = BCrypt.Net.BCrypt.HashPassword(obj.Passwordhashed);

                Profile newUser = new Profile();
                newUser.Firstname = obj.Firstname;
                newUser.Lastname = obj.Lastname;
                newUser.Username = obj.Username;
                newUser.Profileimg = filename;
                newUser.Countryid = obj.Countryid;
                newUser.Stateid = obj.Stateid;
                newUser.Cityid = obj.Cityid;
                newUser.Zipcode = obj.Zipcode;
                newUser.Address = obj.Address;
                newUser.Phoneno = obj.Phoneno;
                newUser.Email = obj.Email;
                newUser.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                newUser.Createdat = DateTime.Now;

                UserLogin newUserLogin = new UserLogin();
                newUserLogin.Isdeleted = false;
                newUserLogin.Passwordhashed = password_Hash;

                UserRole newUserRole = new UserRole();
                newUserRole.Roleid = obj.Roleid;

                bool status = await _userRepository.AddUser(newUser , newUserLogin , newUserRole);

                if(status){    
                    string message = $@"
                    <html>
                        <body>
                            <div style='width: 40vw;'>
                            <p>Welcome To Pizza Shop</p>
                            <p>Please find the details below for login into your account</p>
                            <div style='border: 1px solid black;'>
                            <h1>Login Details</h1>
                            <h3>Username: {obj.Username}</h3>
                            <h3>Temporary Password: {obj.Passwordhashed}</h3>
                            </div>
                            <p>If you encounter any issue or have any quetion, please do'nt hezitate to contact our<br>support team.</p>
                            </div>
                        </body>
                    </html>
                    ";
                    await _emailService.SendAsyncEmail(obj.Email , "User Credentials" , message);
                    return true;
                }
                else{
                    return false;
                }
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region EditUserAsync
        public async Task<bool> EditUserAsync(EditUserViewModel obj){
            try
            {    
                string filename = "";
                string filepath = "";
                if(obj.profileimg != null){
                    string folder = Path.Combine(_env.WebRootPath , "ProfileImages");
                    filename = Guid.NewGuid().ToString() + "_" + obj.profileimg.FileName;
                    filepath = Path.Combine(folder , filename);
                    obj.profileimg.CopyTo(new FileStream(filepath , FileMode.Create));
                }
                Profile editedUser = await _userRepository.GetUserByUserId(obj.Userid);

                editedUser.Firstname = obj.Firstname;
                editedUser.Lastname = obj.Lastname;
                editedUser.Username = obj.Username;
                editedUser.Email = obj.Email;
                editedUser.Status = obj.Status;
                if(filename != ""){
                    editedUser.Profileimg = filename;
                }
                else{
                    editedUser.Profileimg = obj.ExistingProfileImg;
                }
                editedUser.Countryid = obj.Countryid;
                editedUser.Stateid = obj.Stateid;
                editedUser.Cityid = obj.Cityid;
                editedUser.Zipcode = obj.Zipcode;
                editedUser.Address = obj.Address;
                editedUser.Phoneno = obj.Phoneno;
                editedUser.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                editedUser.Updatedat = DateTime.Now;

                UserRole editedUserRole = editedUser.UserRoles.FirstOrDefault();

                editedUserRole.Roleid = obj.Roleid;
                editedUserRole.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                editedUserRole.Updatedat = DateTime.Now;

                bool status = await _userRepository.EditUser(editedUser , editedUserRole);
                return status;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);                
                return false;
            }
        }
    #endregion

    #region ProfileViewModelAsync
        public async Task<bool> ProfileViewModelAsync(ProfileViewModel obj){
            try
            {
                string filename = "";
                string filepath = "";
                if(obj.ProfileImg != null){
                    string folder = Path.Combine(_env.WebRootPath , "ProfileImages");
                    filename = Guid.NewGuid().ToString() + "_" + obj.ProfileImg.FileName;
                    filepath = Path.Combine(folder , filename);
                    obj.ProfileImg.CopyTo(new FileStream(filepath , FileMode.Create));
                }
                Profile check = await _userRepository.GetUserByUserId(obj.Userid);
                check.Firstname = obj.Firstname;
                check.Lastname = obj.Lastname;
                check.Username = obj.Username;
                check.Phoneno = obj.Phoneno;
                check.Address = obj.Address;
                check.Zipcode = obj.Zipcode;
                check.Countryid = obj.Countryid;
                check.Stateid = obj.Stateid;
                check.Cityid = obj.Cityid;
                check.Email = obj.Email;
                if(filename != ""){
                    check.Profileimg = filename;
                }
                check.Updatedby = obj.Userid;
                check.Updatedat = DateTime.Now;
                return await _userRepository.ChangeProfile(check);
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region GetEditUserAsync
        public async Task<EditUserViewModel> GetEditUserAsync(int userid)
        {
            Profile check = await _userRepository.AddDataToEditUserViewModel(userid);
            EditUserViewModel obj = new EditUserViewModel();
            obj.Userid = check.Userid;
            obj.Firstname = check.Firstname;
            obj.Lastname = check.Lastname;
            obj.Username = check.Username;
            obj.Roleid = (int)check.UserRoles.First().Roleid;
            obj.Email = check.Email;
            obj.Status = (bool)check.Status;
            obj.ExistingProfileImg = check.Profileimg;
            // obj.profileimg = check.Profileimg;
            obj.Countryid = check.Countryid;
            obj.Stateid = check.Stateid;
            obj.Cityid = check.Cityid;
            obj.Zipcode = check.Zipcode;
            obj.Address = check.Address;
            obj.Phoneno = check.Phoneno;
            return obj;
        }
    #endregion

    #region GetSelectedStatesCities
        public async Task<(SelectList states , SelectList cities)> GetSelectedStatesCities(int countryId , int statesId){
            return (new SelectList(await _userRepository.GetState(countryId) , "Stateid","StateName" ) , new SelectList(await _userRepository.GetCity(statesId) , "Cityid", "CityName"));
        }
    #endregion

    #region GetProfileViewModelAsync
        public async Task<ProfileViewModel> GetProfileViewModelAsync(){
            Profile check = await _userRepository.AddDataToProfileViewModel(Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")));
            ProfileViewModel obj = new ProfileViewModel();
            obj.Userid = check.Userid;
            obj.Firstname = check.Firstname;
            obj.Lastname = check.Lastname;
            obj.Username = check.Username;
            obj.Phoneno = check.Phoneno;
            obj.Address = check.Address;
            obj.Zipcode = check.Zipcode;
            obj.Countryid = check.Countryid;
            obj.CountryName = check.Country.CountryName;
            obj.Stateid = check.Stateid;
            obj.StateName = check.State.StateName;
            obj.Cityid = check.Cityid;
            obj.CityName = check.City.CityName;
            obj.Email = check.Email;
            obj.ExistingProfileImg = check.Profileimg;
            obj.RoleName = check.UserRoles.First().Role.Roles;
            return obj;
        }
    #endregion

}
