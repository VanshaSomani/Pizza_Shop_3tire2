namespace Pizza_Shop_Services.Bean;

public class Constant
{
    #region success
        public const string SuccessAdd = "{0} Added Successfully";
        public const string SuccessEdit = "{0} Edited Successfully";
        public const string SuccessDelete = "{0} Delete Successfully";
    #endregion

    #region error
        public const string ErrorAdd = "Error adding {0}";
        public const string ErrorEdit = "Error editing {0}";
        public const string ErrorDelete = "Error deleting {0}";
    #endregion

    #region invalid
        public const string IdInValid = "{0}id is invalid";
    #endregion

    #region Comman message
        public const string GeneralSuccess = "Success";
        public const string GeneralError = "Something went wrong";
    #endregion

    #region Login message
        public const string LoginInvalid = "Email or password is invalid";
        public const string LoginSuccess = "Login Successfull";
        public const string AuthorizationFail = "Authorization Failed";
        public const string LoginFail = "Login Fails";
        public const string ResetEmailSent = "Reset email sent";
        public const string InvalidEmail = "No such email exist";
        public const string PasswordResetSuccess = "Password reset successful";
        public const string PasswordResetFail = "PasswordReset Faild";
    #endregion

    #region pagename
        public const string User = "User";
        public const string Reset = "Reset";
        public const string Profile = "Profile";
        public const string Login = "Login";
        public const string Password = "Password";
        public const string RoleAndPermission = "Role And Permission";
        public const string Permission = "Permission";
        public const string Category = "Category";
        public const string Item = "Item";
        public const string Modifier = "Modifier";
        public const string ModifierGroup = "Modifier Group";
        public const string Table = "Table";
        public const string Section = "Section";
        public const string Tax = "Taxes";
        public const string Order = "Order";
        public const string Customer = "Customer";
        public const string KOT = "KOT";
        public const string WaitingList = "Waiting List";
    #endregion
}
