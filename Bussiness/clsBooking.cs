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
    public class clsBooking
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int BookingID { get; set; }
        public int ReservationID { get; set; }
        public int GuestID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public byte Status { get; set; }

        public clsBooking()
        {
            this.BookingID = -1;
            this.ReservationID = -1;
            this.GuestID = -1;
            this.CheckInDate = DateTime.Now;
            this.CheckOutDate = DateTime.Now;
            this.Status = 0;

            Mode = enMode.AddNew;
        }

        private clsBooking(int BookingID, int ReservationID, int GuestID, DateTime CheckInDate,
            DateTime CheckOutDate, byte Status)
        {
            this.BookingID = BookingID;
            this.ReservationID = ReservationID;
            this.GuestID = GuestID;
            this.CheckInDate = CheckInDate;
            this.CheckOutDate = CheckOutDate;
            this.Status = Status;

            Mode = enMode.Update;
        }

        private bool _AddNewBooking()
        {
            bool result = clsBookingData.AddNewBooking(this.ReservationID, this.GuestID, this.CheckInDate,
                this.CheckOutDate, this.Status);

            return result;
        }

        private bool _UpdateBooking()
        {
            return clsBookingData.UpdateBooking(this.BookingID, this.ReservationID, this.GuestID,
                this.CheckInDate, this.CheckOutDate, this.Status);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewBooking())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateBooking();
            }

            return false;
        }

        public static clsBooking Find(int BookingID)
        {
            int ReservationID = -1, GuestID = -1;
            DateTime CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now;
            byte Status = 0;

            bool IsFound = clsBookingData.GetBookingInfoByID(BookingID, ref ReservationID, ref GuestID,
                ref CheckInDate, ref CheckOutDate, ref Status);

            if (IsFound)
            {
                return new clsBooking(BookingID, ReservationID, GuestID, CheckInDate, CheckOutDate, Status);
            }
            else
            {
                return null;
            }
        }

        public static bool DeleteBooking(int BookingID)
        {
            return clsBookingData.DeleteBooking(BookingID);
        }

        public static bool DoesBookingExist(int BookingID)
        {
            return clsBookingData.DoesBookingExist(BookingID);
        }

        public static DataTable GetAllBookings()
        {
            return clsBookingData.GetAllBookings();
        }

        public static int GetBookingsCount()
        {
            return clsBookingData.GetBookingsCount();
        }
    }
}