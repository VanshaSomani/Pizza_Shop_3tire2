namespace Pizza_Shop_Services.Bean;

public class MessageHelper
{
    #region Success
        public static string GetAddSuccessMessage(string page){
            if(String.IsNullOrEmpty(page)){
                return Constant.GeneralSuccess;
            }
            return string.Format(Constant.SuccessAdd, page);
        }

        public static string GetEditSuccessMessage(string page){
            if(String.IsNullOrEmpty(page)){
                return Constant.GeneralSuccess;
            }
            return string.Format(Constant.SuccessEdit, page);
        }

        public static string GetDeleteSuccessMessage(string page){
            if(String.IsNullOrEmpty(page)){
                return Constant.GeneralSuccess;
            }
            return string.Format(Constant.SuccessDelete, page);
        }
    #endregion

    #region Name
        public static string GetInvalidErrorMessage(string page){
            if(String.IsNullOrEmpty(page)){
                return Constant.GeneralError;
            }
            return string.Format(Constant.IdInValid, page);
        }
    #endregion

    #region Error
        public static string GetAddErrorMessage(string page){
            if(String.IsNullOrEmpty(page)){
                return Constant.GeneralError;
            }
            return string.Format(Constant.ErrorAdd, page);
        }

        public static string GetEditErrorMessage(string page){
            if(String.IsNullOrEmpty(page)){
                return Constant.GeneralError;
            }
            return string.Format(Constant.ErrorEdit, page);
        }

        public static string GetDeleteErrorMessage(string page){
            if(String.IsNullOrEmpty(page)){
                return Constant.GeneralError;
            }
            return string.Format(Constant.ErrorDelete, page);
        }
    #endregion
}
