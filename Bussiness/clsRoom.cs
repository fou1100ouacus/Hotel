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
    public class clsRooms
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int RoomID { get; set; }
        public int RoomTypeID { get; set; }
        public string RoomNumber { get; set; }
        public byte RoomFloor { get; set; }
        public decimal RoomSize { get; set; }
        public byte AvailabilityStatus { get; set; }
        public bool IsSmokingAllowed { get; set; }
        public bool IsPetFriendly { get; set; }
        public string AdditionalNotes { get; set; }

        public clsRooms()
        {
            this.RoomID = -1;
            this.RoomTypeID = 0;
            this.RoomNumber = string.Empty;
            this.RoomFloor = 0;
            this.RoomSize = 0.0m;
            this.AvailabilityStatus = 0;
            this.IsSmokingAllowed = false;
            this.IsPetFriendly = false;
            this.AdditionalNotes = string.Empty;

            Mode = enMode.AddNew;
        }

        private clsRooms(int RoomID, int RoomTypeID, string RoomNumber, byte RoomFloor,
            decimal RoomSize, byte AvailabilityStatus, bool IsSmokingAllowed, bool IsPetFriendly,
            string AdditionalNotes)
        {
            this.RoomID = RoomID;
            this.RoomTypeID = RoomTypeID;
            this.RoomNumber = RoomNumber;
            this.RoomFloor = RoomFloor;
            this.RoomSize = RoomSize;
            this.AvailabilityStatus = AvailabilityStatus;
            this.IsSmokingAllowed = IsSmokingAllowed;
            this.IsPetFriendly = IsPetFriendly;
            this.AdditionalNotes = AdditionalNotes;

            Mode = enMode.Update;
        }

        private bool _AddNewRoom()
        {
            return RoomData.AddNewRoom(this.RoomTypeID, this.RoomNumber, this.RoomFloor,
                this.RoomSize, this.AvailabilityStatus, this.IsSmokingAllowed, this.IsPetFriendly,
                this.AdditionalNotes);
        }

        private bool _UpdateRoom()
        {
            return DataAccess.RoomData.UpdateRoom((int)this.RoomID, this.RoomTypeID, this.RoomNumber,
                this.RoomFloor, this.RoomSize, this.AvailabilityStatus, this.IsSmokingAllowed,
                this.IsPetFriendly, this.AdditionalNotes);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewRoom())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateRoom();
            }
            return false;
        }

        public static clsRooms Find(int RoomID)
        {
            int RoomTypeID = 0;
            string RoomNumber = string.Empty;
            byte RoomFloor = 0;
            decimal RoomSize = 0.0m;
            byte AvailabilityStatus = 0;
            bool IsSmokingAllowed = false;
            bool IsPetFriendly = false;
            string AdditionalNotes = string.Empty;

            bool IsFound = DataAccess.RoomData.GetRoomInfoByID(RoomID, ref RoomTypeID, ref RoomNumber,
                ref RoomFloor, ref RoomSize, ref AvailabilityStatus, ref IsSmokingAllowed,
                ref IsPetFriendly, ref AdditionalNotes);

            if (IsFound)
            {
                return new clsRooms(RoomID, RoomTypeID, RoomNumber, RoomFloor,
                    RoomSize, AvailabilityStatus, IsSmokingAllowed, IsPetFriendly,
                    AdditionalNotes);
            }
            else
            {
                return null;
            }
        }

        public static bool DoesRoomExist(int RoomID)
        {
            return DataAccess.RoomData.DoesRoomExist(RoomID);
        }

        public static bool DoesRoomExist(string RoomNumber)
        {
            return DataAccess.RoomData.DoesRoomExist(RoomNumber);
        }

        public static bool DeleteRoom(int RoomID)
        {
            if (DataAccess.RoomData.DeleteRoom(RoomID))
            {
                return true;
            }
            return false;
        }

        public static DataTable GetAllRooms()
        {
            return DataAccess.RoomData.GetAllRooms();
        }

        public static int GetRoomsCount()
        {
            return DataAccess.RoomData.GetRoomsCount();
        }
    }
}