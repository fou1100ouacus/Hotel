using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class RoomData
    {
        public static bool GetRoomInfoByID(int RoomID, ref int RoomTypeID, ref string RoomNumber,
            ref byte RoomFloor, ref decimal RoomSize, ref byte AvailabilityStatus, ref bool IsSmokingAllowed,
            ref bool IsPetFriendly, ref string AdditionalNotes)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select * from Rooms where RoomID = @RoomID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomID", RoomID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                RoomTypeID = (int)reader["RoomTypeID"];
                                RoomNumber = (string)reader["RoomNumber"];
                                RoomFloor = (byte)reader["RoomFloor"];
                                RoomSize = (decimal)reader["RoomSize"];
                                AvailabilityStatus = (byte)reader["AvailabilityStatus"];
                                IsSmokingAllowed = (bool)reader["IsSmokingAllowed"];
                                IsPetFriendly = (bool)reader["IsPetFriendly"];
                                AdditionalNotes = reader["AdditionalNotes"] != DBNull.Value ? (string)reader["AdditionalNotes"] : null;
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

        public static bool AddNewRoom(int RoomTypeID, string RoomNumber, byte RoomFloor,
            decimal RoomSize, byte AvailabilityStatus, bool IsSmokingAllowed, bool IsPetFriendly, string AdditionalNotes)
        {
            int RoomID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"if not Exists (select found = 1 from Rooms where RoomNumber = @RoomNumber)
    begin
    insert into Rooms (RoomTypeID, RoomNumber, RoomFloor, RoomSize, AvailabilityStatus, IsSmokingAllowed, IsPetFriendly, AdditionalNotes)
    values (@RoomTypeID, @RoomNumber, @RoomFloor, @RoomSize, @AvailabilityStatus, @IsSmokingAllowed, @IsPetFriendly, @AdditionalNotes)
    select scope_identity()
    end";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomTypeID", RoomTypeID);
                        command.Parameters.AddWithValue("@RoomNumber", RoomNumber);
                        command.Parameters.AddWithValue("@RoomFloor", RoomFloor);
                        command.Parameters.AddWithValue("@RoomSize", RoomSize);
                        command.Parameters.AddWithValue("@AvailabilityStatus", AvailabilityStatus);
                        command.Parameters.AddWithValue("@IsSmokingAllowed", IsSmokingAllowed);
                        command.Parameters.AddWithValue("@IsPetFriendly", IsPetFriendly);
                        command.Parameters.AddWithValue("@AdditionalNotes", (object)AdditionalNotes ?? DBNull.Value);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertID))
                        {
                            RoomID = InsertID;
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

            return RoomID != -1;
        }

        public static bool UpdateRoom(int RoomID, int RoomTypeID, string RoomNumber, byte RoomFloor,
            decimal RoomSize, byte AvailabilityStatus, bool IsSmokingAllowed, bool IsPetFriendly, string AdditionalNotes)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Update Rooms
    set 
    RoomTypeID = @RoomTypeID,
    RoomNumber = @RoomNumber,
    RoomFloor = @RoomFloor,
    RoomSize = @RoomSize,
    AvailabilityStatus = @AvailabilityStatus,
    IsSmokingAllowed = @IsSmokingAllowed,
    IsPetFriendly = @IsPetFriendly,
    AdditionalNotes = @AdditionalNotes
    where RoomID = @RoomID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomID", RoomID);
                        command.Parameters.AddWithValue("@RoomTypeID", RoomTypeID);
                        command.Parameters.AddWithValue("@RoomNumber", RoomNumber);
                        command.Parameters.AddWithValue("@RoomFloor", RoomFloor);
                        command.Parameters.AddWithValue("@RoomSize", RoomSize);
                        command.Parameters.AddWithValue("@AvailabilityStatus", AvailabilityStatus);
                        command.Parameters.AddWithValue("@IsSmokingAllowed", IsSmokingAllowed);
                        command.Parameters.AddWithValue("@IsPetFriendly", IsPetFriendly);
                        command.Parameters.AddWithValue("@AdditionalNotes", (object)AdditionalNotes ?? DBNull.Value);

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

        public static bool DeleteRoom(int? RoomID)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"delete Rooms where RoomID = @RoomID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomID", (object)RoomID ?? DBNull.Value);

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

        public static DataTable GetAllRooms()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select * from Rooms order by RoomID desc";

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

        public static int GetRoomsCount()
        {
            int Count = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select count(*) from Rooms";

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

        public static bool DoesRoomExist(int RoomID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select found = 1 from Rooms where RoomID = @RoomID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomID", RoomID);

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

        public static bool DoesRoomExist(string RoomNumber)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select found = 1 from Rooms where RoomNumber = @RoomNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomNumber", RoomNumber);

                        object result = command.ExecuteScalar();

                        IsFound = (result != null);
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
    }
}