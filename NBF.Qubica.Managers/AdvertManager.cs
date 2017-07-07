using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NBF.Qubica.Classes;
using MySql.Data.MySqlClient;
using NBF.Qubica.Common;
using NBF.Qubica.Database;

namespace NBF.Qubica.Managers
{
    public static class AdvertManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Advert DataToObject(MySqlDataReader dataReader)
        {
            S_Advert advert = new S_Advert();

            advert.id = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
            advert.advertisement = Conversion.SqlToString(dataReader["advertisement"]);
            advert.advertisement_url = Conversion.SqlToString(dataReader["advertisement_url"]);
            advert.advertisement_www = Conversion.SqlToString(dataReader["advertisement_www"]);
            advert.bowlingcenterId = Conversion.SqlToLongOrNull(dataReader["bowlingcenter_id"]).Value;

            return advert;
        }

        public static S_Advert GetAdvertById(long id)
        {
            S_Advert advert = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM advert WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        advert = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetAdvertById, Error reading advert data: {0}", ex.Message));
            }

            return advert;
        }

        public static S_Advert GetRandomAdvert()
        {
            S_Advert advert = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM advert ORDER BY RAND() LIMIT 1";

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        advert = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetRandomAdvert, Error reading advert data: {0}", ex.Message));
            }

            return advert;
        }

        public static bool AdvertExistById(long id)
        {
            bool result = false;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM advert WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    result = dataReader.HasRows;

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("AdvertExistById, Error reading advert data: {0}", ex.Message));
            }

            return result;
        }

        public static List<S_Advert> GetAdvertsByBowlingCenterid(long id)
        {
            List<S_Advert> adverts = new List<S_Advert>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM advert WHERE bowlingcenter_id=@bowlingcenter_id";
                    command.Parameters.AddWithValue("@bowlingcenter_id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        adverts.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetAdvertsByBowlingcenterId, Error reading advert data: {0}", ex.Message));
            }

            return adverts;
        }


        //Insert statement
        public static long? Insert(S_Advert advert)
        {
            long? lastInsertedId = null;
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "INSERT INTO advert (bowlingcenter_id, advertisement, advertisement_url, advertisement_www) VALUES (@bowlingcenter_id, @advertisement, @advertisement_url, @advertisement_www)";
                    command.Parameters.AddWithValue("@bowlingcenter_id", Conversion.LongToSql(advert.bowlingcenterId)); 
                    command.Parameters.AddWithValue("@advertisement", Conversion.StringToSql(advert.advertisement));
                    command.Parameters.AddWithValue("@advertisement_url", Conversion.StringToSql(advert.advertisement_url));
                    command.Parameters.AddWithValue("@advertisement_www", Conversion.StringToSql(advert.advertisement_www));

                    //Execute command
                    command.ExecuteNonQuery();
                    lastInsertedId = command.LastInsertedId;

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting advert data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Advert advert)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();

                    command.CommandText = "UPDATE advert SET advertisement=@advertisement, advertisement_url=@advertisement_url, advertisement_www=@advertisement_www WHERE id=@id ";

                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(advert.id));
                    command.Parameters.AddWithValue("@advertisement", Conversion.StringToSql(advert.advertisement));
                    command.Parameters.AddWithValue("@advertisement_url", Conversion.StringToSql(advert.advertisement_url));
                    command.Parameters.AddWithValue("@advertisement_www", Conversion.StringToSql(advert.advertisement_www));

                    //Execute command
                    command.ExecuteNonQuery();

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating advert data: {0}", ex.Message));
            }
        }

        //Delete statement
        public static void Delete(long id)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                if (databaseconnection.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "DELETE FROM advert WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting advert data: {0}", ex.Message));
            }
        }
    }
}
