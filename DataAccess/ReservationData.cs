using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class ReservationData
    {
        public static bool GetReservationInfoByID(int ReservationID, ref int RoomID,
            ref byte NumberOfPeople, ref byte Status, ref DateTime LastStatusDate,
            ref int CustomerID, ref DateTime CreatedDate)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select * from Reservations where ReservationID = @ReservationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationID", ReservationID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                RoomID = (int)reader["RoomID"];
                                NumberOfPeople = (byte)reader["NumberOfPeople"];
                                Status = (byte)reader["Status"];
                                LastStatusDate = (DateTime)reader["LastStatusDate"];
                                CustomerID = (int)reader["CustomerID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];
                            }
                            else
                            {
                                IsFound = false;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                IsFound = false;
            }
            catch (Exception ex)
            {
                IsFound = false;
            }

            return IsFound;
        }

        public static bool AddNewReservation(int ReservationID, int RoomID, byte NumberOfPeople,
            byte Status, DateTime LastStatusDate, int CustomerID, DateTime CreatedDate)
        {
            int NewReservationID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"insert into Reservation (ReservationID, RoomID, NumberOfPeople, 
                        Status, LastStatusDate, CustomerID, CreatedDate)
                        values (@ReservationID, @RoomID, @NumberOfPeople, @Status, @LastStatusDate, 
                        @CustomerID, @CreatedDate);
                        select scope_identity()";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationID", ReservationID);
                        command.Parameters.AddWithValue("@RoomID", RoomID);
                        command.Parameters.AddWithValue("@NumberOfPeople", NumberOfPeople);
                        command.Parameters.AddWithValue("@Status", Status);
                        command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                        command.Parameters.AddWithValue("@CustomerID", CustomerID);
                        command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertID))
                        {
                            NewReservationID = InsertID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {
            }

            return NewReservationID != -1;
        }

        public static bool UpdateReservation(int ReservationID, int RoomID,
            byte NumberOfPeople, byte Status, DateTime LastStatusDate, int CustomerID, DateTime CreatedDate)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Update Reservation
                        set 
                        RoomID = @RoomID,
                        NumberOfPeople = @NumberOfPeople,
                        Status = @Status,
                        LastStatusDate = @LastStatusDate,
                        CustomerID = @CustomerID,
                        CreatedDate = @CreatedDate
                        where ReservationID = @ReservationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationID", ReservationID);
                        command.Parameters.AddWithValue("@RoomID", RoomID);
                        command.Parameters.AddWithValue("@NumberOfPeople", NumberOfPeople);
                        command.Parameters.AddWithValue("@Status", Status);
                        command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                        command.Parameters.AddWithValue("@CustomerID", CustomerID);
                        command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

                        RowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {
            }

            return (RowAffected > 0);
        }

        public static bool DeleteReservation(int? ReservationID)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"delete Reservation where ReservationID = @ReservationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationID", (object)ReservationID ?? DBNull.Value);

                        RowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {
            }

            return (RowAffected > 0);
        }

        public static DataTable GetAllReservations()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select ReservationID, RoomID, NumberOfPeople, Status, 
                        LastStatusDate, CustomerID, CreatedDate 
                        from Reservation order by ReservationID desc";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {
            }

            return dt;
        }

        public static int GetReservationsCount()
        {
            int Count = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select count(*) from Reservations";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int Value))
                        {
                            Count = Value;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {
            }

            return Count;
        }

        public static bool DoesReservationExist(int ReservationID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select found = 1 from Reservations where ReservationID = @ReservationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationID", ReservationID);

                        object result = command.ExecuteScalar();

                        IsFound = (result != null);
                    }
                }
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {
            }

            return IsFound;
        }
    }
}