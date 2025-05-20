using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace BusinessLayer
{
    public enum enReservationStatus : byte
    {
        Pending = 0,
        Confirmed = 1,
        Canceled = 2,
        Completed = 3,
        NoShow = 4
    }

    public class clsReservation
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ReservationID { get; set; }
        public int RoomID { get; set; }
        public byte NumberOfPeople { get; set; }
        public enReservationStatus Status { get; set; }
        public DateTime LastStatusDate { get; set; }
        public int CustomerID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsReservation()
        {
            this.ReservationID = -1;
            this.RoomID = -1;
            this.NumberOfPeople = 0;
            this.Status = enReservationStatus.Pending;
            this.LastStatusDate = DateTime.MinValue;
            this.CustomerID = -1;
            this.CreatedDate = DateTime.MinValue;

            Mode = enMode.AddNew;
        }

        private clsReservation(int ReservationID, int RoomID, byte NumberOfPeople, enReservationStatus Status,
            DateTime LastStatusDate, int CustomerID, DateTime CreatedDate)
        {
            this.ReservationID = ReservationID;
            this.RoomID = RoomID;
            this.NumberOfPeople = NumberOfPeople;
            this.Status = Status;
            this.LastStatusDate = LastStatusDate;
            this.CustomerID = CustomerID;
            this.CreatedDate = CreatedDate;

            Mode = enMode.Update;
        }

        private bool _AddNewReservation()
        {
            int newReservationID = -1;
            bool result = ReservationData.AddNewReservation(this.ReservationID, this.RoomID, this.NumberOfPeople,
                (byte)this.Status, this.LastStatusDate, this.CustomerID, this.CreatedDate);

            if (result)
            {
                this.ReservationID = newReservationID; // Note: Requires ReservationData.AddNewReservation to return ID
                return true;
            }
            return false;
        }

        private bool _UpdateReservation()
        {
            return ReservationData.UpdateReservation(this.ReservationID, this.RoomID, this.NumberOfPeople,
                (byte)this.Status, this.LastStatusDate, this.CustomerID, this.CreatedDate);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewReservation())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateReservation();
            }

            return false;
        }

        public static clsReservation Find(int ReservationID)
        {
            int RoomID = -1;
            byte NumberOfPeople = 0;
            byte Status = 0;
            DateTime LastStatusDate = DateTime.MinValue;
            int CustomerID = -1;
            DateTime CreatedDate = DateTime.MinValue;

            bool IsFound = ReservationData.GetReservationInfoByID(ReservationID, ref RoomID, ref NumberOfPeople,
                ref Status, ref LastStatusDate, ref CustomerID, ref CreatedDate);

            if (IsFound)
            {
                return new clsReservation(ReservationID, RoomID, NumberOfPeople, (enReservationStatus)Status,
                    LastStatusDate, CustomerID, CreatedDate);
            }
            else
            {
                return null;
            }
        }

        public static bool DeleteReservation(int ReservationID)
        {
            return ReservationData.DeleteReservation(ReservationID);
        }

        public static bool DoesReservationExist(int ReservationID)
        {
            return ReservationData.DoesReservationExist(ReservationID);
        }

        public static DataTable GetAllReservations()
        {
            return ReservationData.GetAllReservations();
        }

        public static int GetReservationsCount()
        {
            return ReservationData.GetReservationsCount();
        }
    }
}