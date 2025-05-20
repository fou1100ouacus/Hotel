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
    public class clsPayment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PaymentID { get; set; }
        public int BookingID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaidAmount { get; set; }
        public int CreatedByUserID { get; set; }

        public clsPayment()
        {
            this.PaymentID = -1;
            this.BookingID = -1;
            this.PaymentDate = DateTime.Now;
            this.PaidAmount = 0.0m;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsPayment(int PaymentID, int BookingID, DateTime PaymentDate, decimal PaidAmount, int CreatedByUserID)
        {
            this.PaymentID = PaymentID;
            this.BookingID = BookingID;
            this.PaymentDate = PaymentDate;
            this.PaidAmount = PaidAmount;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;
        }

        private bool _AddNewPayment()
        {
            bool result = clsPaymentData.AddNewPayment(this.BookingID, this.PaymentDate, this.PaidAmount, this.CreatedByUserID);

            return result;
        }

        private bool _UpdatePayment()
        {
            return clsPaymentData.UpdatePayment(this.PaymentID, this.BookingID, this.PaymentDate, this.PaidAmount, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPayment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePayment();
            }

            return false;
        }

        public static clsPayment Find(int PaymentID)
        {
            int BookingID = -1, CreatedByUserID = -1;
            DateTime PaymentDate = DateTime.Now;
            decimal PaidAmount = 0.0m;

            bool IsFound = clsPaymentData.GetPaymentInfoByID(PaymentID, ref BookingID, ref PaymentDate, ref PaidAmount, ref CreatedByUserID);

            if (IsFound)
            {
                return new clsPayment(PaymentID, BookingID, PaymentDate, PaidAmount, CreatedByUserID);
            }
            else
            {
                return null;
            }
        }

        public static bool DeletePayment(int PaymentID)
        {
            return clsPaymentData.DeletePayment(PaymentID);
        }

        public static bool DoesPaymentExist(int PaymentID)
        {
            return clsPaymentData.DoesPaymentExist(PaymentID);
        }

        public static DataTable GetAllPayments()
        {
            return clsPaymentData.GetAllPayments();
        }

        public static int GetPaymentsCount()
        {
            return clsPaymentData.GetPaymentsCount();
        }
    }
}