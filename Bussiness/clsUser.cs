using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace BussinessLayer
{
    public class clsUser
    {
        
            public enum enMode { AddNew = 0, Update = 1 };
            public enMode Mode = enMode.AddNew;

           
            public int     UserID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
      
            public clsUser()
            {
                this.UserID = -1;
                this.Username = string.Empty;
                this.Password = string.Empty;
              

                Mode = enMode.AddNew;
            }

            private clsUser(int UserID, string Username, string Password )
            {
              
                this.UserID = UserID;
                this.Username = Username;
                this.Password = Password;
              
                Mode = enMode.Update;
            }

            private bool _AddNewUser()
            {
                return clsUserData.AddNewUser(  this.Username, this.Password  );

               
            }

            private bool _UpdateUser()
            {
                return DataAccess.clsUserData.UpdateUser((int)this.UserID, this.Username, this.Password);
            }

            public bool Save()
            {

                
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewUser())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case enMode.Update:
                        return _UpdateUser();
                }

                return false;
            }

        



            public static clsUser Find(string Username, string Password)
            {
                int UserID = -1;
              

                bool IsFound = DataAccess.clsUserData.GetUserInfoByUsernameAndPassword(ref UserID,Username, Password);

                if (IsFound)
                {


                    return new clsUser(  UserID, Username, Password    );
                }
                else
                {
                    return null;
                }
            }
        public static bool DoesUserExist(string Username, string Password)
        {
            return clsUserData.DoesUserExist(Username, Password);
        }
        public static bool DeleteUser(int UserID)
            {
               

                if (DataAccess.clsUserData.DeleteUser(UserID))
                {
                return true;
                }

                return false;
            }

            public static bool DoesUserExist(int UserID)
            {
                return DataAccess.clsUserData.DoesUserExist(UserID);
            }

            public static bool DoesUserExist(string Username)
            {
                return DataAccess.clsUserData.DoesUserExist(Username);
            }

    

            public static DataTable GetAllUsers()
            {
                return DataAccess.clsUserData.GetAllUsers();
            }

            public static int GetUsersCount()
            {
                return DataAccess.clsUserData.GetUsersCount();
            }

            public bool ChangePassword(string NewPassword)
            {
                return ChangePassword(this.UserID, NewPassword);
            }

            public static bool ChangePassword(int? UserID, string NewPassword)
            {
                return DataAccess.clsUserData.ChangePassword(UserID, NewPassword);
            }
        
    
    }
    

}
