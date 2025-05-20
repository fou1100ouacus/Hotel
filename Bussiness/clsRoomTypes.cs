using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace BussinessLayer
{


        public class clsRoomTypes
        {
            public enum enMode { AddNew = 0, Update = 1 };
            public enMode Mode = enMode.AddNew;

            public int RoomTypeID { get; set; }
            public string RoomTypeTitle { get; set; }
            public byte RoomTypeCapacity { get; set; }
            public decimal RoomTypePricePerNight { get; set; }
            public string RoomTypeDescription { get; set; }

            public clsRoomTypes()
            {
                this.RoomTypeID = -1;
                this.RoomTypeTitle = string.Empty;
                this.RoomTypeCapacity = 0;
                this.RoomTypePricePerNight = 0.0m;
                this.RoomTypeDescription = string.Empty;

                Mode = enMode.AddNew;
            }

            private clsRoomTypes(int RoomTypeID, string RoomTypeTitle, byte RoomTypeCapacity,
                decimal RoomTypePricePerNight, string RoomTypeDescription)
            {
                this.RoomTypeID = RoomTypeID;
                this.RoomTypeTitle = RoomTypeTitle;
                this.RoomTypeCapacity = RoomTypeCapacity;
                this.RoomTypePricePerNight = RoomTypePricePerNight;
                this.RoomTypeDescription = RoomTypeDescription;

                Mode = enMode.Update;
            }

            private bool _AddNewRoomType()
            {
                return RoomTypesData.AddNewRoomType(this.RoomTypeTitle, this.RoomTypeCapacity,
                    this.RoomTypePricePerNight, this.RoomTypeDescription);
            }

            private bool _UpdateRoomType()
            {
                return DataAccess.RoomTypesData.UpdateRoomType((int)this.RoomTypeID, this.RoomTypeTitle,
                    this.RoomTypeCapacity, this.RoomTypePricePerNight, this.RoomTypeDescription);
            }

            public bool Save()
            {
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewRoomType())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case enMode.Update:
                        return _UpdateRoomType();
                }
                return false;
            }

            public static clsRoomTypes Find(int RoomTypeID)
            {
                string RoomTypeTitle = string.Empty;
                byte RoomTypeCapacity = 0;
                decimal RoomTypePricePerNight = 0.0m;
                string RoomTypeDescription = string.Empty;

                bool IsFound = DataAccess.RoomTypesData.GetRoomTypeInfoByID(RoomTypeID, ref RoomTypeTitle,
                    ref RoomTypeCapacity, ref RoomTypePricePerNight, ref RoomTypeDescription);

                if (IsFound)
                {
                    return new clsRoomTypes(RoomTypeID, RoomTypeTitle, RoomTypeCapacity,
                        RoomTypePricePerNight, RoomTypeDescription);
                }
                else
                {
                    return null;
                }
            }

            public static bool DoesRoomTypeExist(int RoomTypeID)
            {
                return DataAccess.RoomTypesData.DoesRoomTypeExist(RoomTypeID);
            }

            public static bool DoesRoomTypeExist(string RoomTypeTitle)
            {
                return DataAccess.RoomTypesData.DoesRoomTypeExist(RoomTypeTitle);
            }

            public static bool DeleteRoomType(int RoomTypeID)
            {
                if (DataAccess.RoomTypesData.DeleteRoomType(RoomTypeID))
                {
                    return true;
                }
                return false;
            }

            public static System.Data.DataTable GetAllRoomTypes()
            {
                return DataAccess.RoomTypesData.GetAllRoomTypes();
            }

            public static int GetRoomTypesCount()
            {
                return DataAccess.RoomTypesData.GetRoomTypesCount();
            }
        public static DataTable GetRoomTypes()
        {
            return DataAccess.RoomTypesData.GetRoomTypes();
        }
    }

    

}





