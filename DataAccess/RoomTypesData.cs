using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
   
        public static class RoomTypesData
        {
            public static bool GetRoomTypeInfoByID(int RoomTypeID, ref string RoomTypeTitle,
                ref byte RoomTypeCapacity, ref decimal RoomTypePricePerNight, ref string RoomTypeDescription)
            {
                bool IsFound = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                    {
                        connection.Open();

                        string query = @"select * from RoomTypes where RoomTypeID = @RoomTypeID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@RoomTypeID", RoomTypeID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    IsFound = true;
                                    RoomTypeTitle = reader["RoomTypeTitle"] != DBNull.Value ? (string)reader["RoomTypeTitle"] : null;
                                    RoomTypeCapacity = (byte)reader["RoomTypeCapacity"];
                                    RoomTypePricePerNight = (decimal)reader["RoomTypePricePerNight"];
                                    RoomTypeDescription = reader["RoomTypeDescription"] != DBNull.Value ? (string)reader["RoomTypeDescription"] : null;
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

            public static bool AddNewRoomType(string RoomTypeTitle, byte RoomTypeCapacity,
                decimal RoomTypePricePerNight, string RoomTypeDescription)
            {
                int RoomTypeID = -1;

                try
                {
                    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                    {
                        connection.Open();

                        string query = @"if not Exists (select found = 1 from RoomTypes where RoomTypeTitle = @RoomTypeTitle)
    begin
    insert into RoomTypes (RoomTypeTitle, RoomTypeCapacity, RoomTypePricePerNight, RoomTypeDescription)
    values (@RoomTypeTitle, @RoomTypeCapacity, @RoomTypePricePerNight, @RoomTypeDescription)
    select scope_identity()
    end";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@RoomTypeTitle", RoomTypeTitle);
                            command.Parameters.AddWithValue("@RoomTypeCapacity", RoomTypeCapacity);
                            command.Parameters.AddWithValue("@RoomTypePricePerNight", RoomTypePricePerNight);
                            command.Parameters.AddWithValue("@RoomTypeDescription", (object)RoomTypeDescription ?? DBNull.Value);

                            object result = command.ExecuteScalar();

                            if (result != null && int.TryParse(result.ToString(), out int InsertID))
                            {
                                RoomTypeID = InsertID;
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

                return RoomTypeID != -1;
            }

            public static bool UpdateRoomType(int RoomTypeID, string RoomTypeTitle, byte RoomTypeCapacity,
                decimal RoomTypePricePerNight, string RoomTypeDescription)
            {
                int RowAffected = 0;

                try
                {
                    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                    {
                        connection.Open();

                        string query = @"Update RoomTypes
    set 
    RoomTypeTitle = @RoomTypeTitle,
    RoomTypeCapacity = @RoomTypeCapacity,
    RoomTypePricePerNight = @RoomTypePricePerNight,
    RoomTypeDescription = @RoomTypeDescription
    where RoomTypeID = @RoomTypeID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@RoomTypeID", RoomTypeID);
                            command.Parameters.AddWithValue("@RoomTypeTitle", RoomTypeTitle);
                            command.Parameters.AddWithValue("@RoomTypeCapacity", RoomTypeCapacity);
                            command.Parameters.AddWithValue("@RoomTypePricePerNight", RoomTypePricePerNight);
                            command.Parameters.AddWithValue("@RoomTypeDescription", (object)RoomTypeDescription ?? DBNull.Value);

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

            public static bool DeleteRoomType(int? RoomTypeID)
            {
                int RowAffected = 0;

                try
                {
                    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                    {
                        connection.Open();

                        string query = @"delete RoomTypes where RoomTypeID = @RoomTypeID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@RoomTypeID", (object)RoomTypeID ?? DBNull.Value);

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

            public static DataTable GetAllRoomTypes()
            {
                DataTable dt = new DataTable();

                try
                {
                    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                    {
                        connection.Open();

                        string query = @"select * from RoomTypes order by RoomTypeID desc";

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
        public static DataTable GetRoomTypes()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select RoomTypeTitle from RoomTypes order by RoomTypeID desc";

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

        public static int GetRoomTypesCount()
            {
                int Count = 0;

                try
                {
                    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                    {
                        connection.Open();

                        string query = @"select count(*) from RoomTypes";

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

            public static bool DoesRoomTypeExist(int RoomTypeID)
            {
                bool IsFound = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                    {
                        connection.Open();

                        string query = @"select found = 1 from RoomTypes where RoomTypeID = @RoomTypeID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@RoomTypeID", RoomTypeID);

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

            public static bool DoesRoomTypeExist(string RoomTypeTitle)
            {
                bool IsFound = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                    {
                        connection.Open();

                        string query = @"select found = 1 from RoomTypes where RoomTypeTitle = @RoomTypeTitle";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@RoomTypeTitle", RoomTypeTitle);

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

