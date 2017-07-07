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
    public static class OpentimeManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Opentime DataToObject(MySqlDataReader dataReader)
        {
            S_Opentime opentime = new S_Opentime();

            opentime.id = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
            opentime.bowlingCenterId = Conversion.SqlToLongOrNull(dataReader["bowlingcenterid"]).Value;
            opentime.day = (Day)Enum.Parse(typeof(Day), Conversion.SqlToString(dataReader["day"]));
            opentime.openTime = Conversion.SqlToString(dataReader["opentime"]);
            opentime.closeTime = Conversion.SqlToString(dataReader["closetime"]);

            return opentime;
        }

        public static List<S_Opentime> GetOpentimesByBowlingcenterId(long bowlingcenterid)
        {
            List<S_Opentime> opentimes = new List<S_Opentime>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM opentime WHERE bowlingcenterid=@bowlingcenterid";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingcenterid));
                    
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        opentimes.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetScores, Error reading opentimes data: {0}", ex.Message));
            }

            return opentimes;
        }

        public static S_Opentime GetOpentimeById(long id)
        {
            S_Opentime scores = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM opentime WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        scores = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetScoreById, Error reading opentimes data: {0}", ex.Message));
            }

            return scores;
        }

        public static bool OpenitemExistById(long id)
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
                    command.CommandText = "SELECT * FROM opentime WHERE id=@id";
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
                logger.Error(string.Format("ScoresExistById, Error reading opentimes data: {0}", ex.Message));
            }

            return result;
        }

        public static bool OpentimesExistByBowlingCenterId(long? bowlingCenterId)
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
                    command.CommandText = "SELECT * FROM opentime WHERE bowlingcenterid=@bowlingcenterid";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingCenterId));
                    
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Check existance
                    if (dataReader.HasRows)
                    {
                        result = dataReader.HasRows;
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("ScoresExistByBowlingCenterId, Error reading openitem data: {0}", ex.Message));
            }

            return result;
        }

        //Insert statement
        public static long? Insert(S_Opentime opentime)
        {
            long? lastInsertedId=null;
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "INSERT INTO opentime (bowlingcenterid, day, opentime, closetime) VALUES (@bowlingcenterid, @day, @opentime, @closetime)";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(opentime.bowlingCenterId));
                    command.Parameters.AddWithValue("@day", Conversion.StringToSql(opentime.day.ToString()));
                    command.Parameters.AddWithValue("@opentime", Conversion.StringToSql(opentime.openTime));
                    command.Parameters.AddWithValue("@closetime", Conversion.StringToSql(opentime.closeTime));

                    //Execute command
                    command.ExecuteNonQuery();
                    lastInsertedId = command.LastInsertedId;
 
                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting opentime data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Opentime scores)
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

                    command.CommandText = "UPDATE opentime SET bowlingcenterid=@bowlingcenterid, day=@day, opentime=@opentime, closetime=@closetime WHERE id=@id ";

                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(scores.id));
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(scores.bowlingCenterId));
                    command.Parameters.AddWithValue("@day", Conversion.StringToSql(scores.day.ToString()));
                    command.Parameters.AddWithValue("@opentime", Conversion.StringToSql(scores.openTime));
                    command.Parameters.AddWithValue("@closetime", Conversion.StringToSql(scores.closeTime));

                    //Execute command
                    command.ExecuteNonQuery();

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating opentime data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM opentime WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting opentime data: {0}", ex.Message));
            }
        }
    }    
}
