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
    public class clsCustomer
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int    CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PassportID { get; set; }

        public clsCustomer()
        {
            this.CustomerID = -1;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Email = string.Empty;
            this.Phone = string.Empty;
            this.Address = string.Empty;
            this.PassportID = string.Empty;

            Mode = enMode.AddNew;
        }

        private clsCustomer(int CustomerID, string FirstName, string LastName, string Email,
            string Phone, string Address, string PassportID)
        {
            this.CustomerID = CustomerID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.PassportID = PassportID;

            Mode = enMode.Update;
        }

        private int _AddNewCustomer()
        {
            int id = ClsCustomerData.AddNewCustomer(this.FirstName, this.LastName, this.Email,  this.Phone, this.Address, this.PassportID);
           
            
            return id ;
        }

        private bool _UpdateCustomer()
        {
            return ClsCustomerData.UpdateCustomer(this.CustomerID, this.FirstName, this.LastName,
                this.Email, this.Phone, this.Address, this.PassportID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCustomer()!=-1)
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateCustomer();
            }

            return false;
        }

        public static clsCustomer Find(int CustomerID)
        {
            string FirstName = "", LastName = "", Email = "", Phone = "", Address = "", PassportID = "";

            bool IsFound = ClsCustomerData.GetCustomerInfoByID(CustomerID, ref FirstName, ref LastName,
                ref Email, ref Phone, ref Address, ref PassportID);

            if (IsFound)
            {
                return new clsCustomer(CustomerID, FirstName, LastName, Email, Phone, Address, PassportID);
            }
            else
            {
                return null;
            }
        }

        public static bool DeleteCustomer(int CustomerID)
        {
            if (ClsCustomerData.DeleteCustomer(CustomerID))
            {
                return true;
            }

            return false;
        }

        public static bool DoesCustomerExist(int CustomerID)
        {
            return ClsCustomerData.DoesCustomerExist(CustomerID);
        }

        public static DataTable GetAllCustomers()
        {
            return ClsCustomerData.GetAllCustomers();
        }

        public static int GetCustomersCount()
        {
            return ClsCustomerData.GetCustomersCount();
        }
    }
}