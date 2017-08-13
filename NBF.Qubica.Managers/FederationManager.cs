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
    public static class FederationManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Federation DataToObject(MySqlDataReader dataReader)
        {
            S_Federation federation = new S_Federation();

            federation.id = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
            federation.information = Conversion.SqlToString(dataReader["information"]);
            federation.logo = Conversion.SqlToString(dataReader["logo"]);

            return federation;
        }

        public static S_Federation GetFederationById(long id)
        {
            S_Federation federation = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM federation WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        federation = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetFederarionById, Error reading federation data: {0}", ex.Message));
            }

            return federation;
        }

        public static S_Federation GetFederation()
        {
            S_Federation federation = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM federation LIMIT 1";

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        federation = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetFederarion, Error reading federation data: {0}", ex.Message));
            }

            return federation;
        }

        public static bool FederationExistById(long id)
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
                    command.CommandText = "SELECT * FROM federation WHERE id=@id";
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
                logger.Error(string.Format("FederationExistById, Error reading federation data: {0}", ex.Message));
            }

            return result;
        }

        //Insert statement
        public static long? Insert(S_Federation federation)
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
                    command.CommandText = "INSERT INTO federation (id, information, logo) VALUES (1, @information, @logo)";
                    command.Parameters.AddWithValue("@information", Conversion.StringToSql(federation.information));
                    command.Parameters.AddWithValue("@logo", Conversion.StringToSql(federation.logo));

                    //Execute command
                    command.ExecuteNonQuery();
                    lastInsertedId = command.LastInsertedId;

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting federation data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Federation federation)
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

                    command.CommandText = "UPDATE federation SET information=@information, logo=@logo WHERE id=@id ";

                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(federation.id));
                    command.Parameters.AddWithValue("@information", Conversion.StringToSql(federation.information));
                    command.Parameters.AddWithValue("@logo", Conversion.StringToSql(federation.logo));

                    //Execute command
                    command.ExecuteNonQuery();

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating federation data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM federation WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting federation data: {0}", ex.Message));
            }
        }
    }
}
