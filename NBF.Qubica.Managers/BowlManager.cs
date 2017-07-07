using MySql.Data.MySqlClient;
using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Database;
using NLog;
using System;
using System.Collections.Generic;

namespace NBF.Qubica.Managers
{
    public static class BowlManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Bowl DataToObject(MySqlDataReader dataReader)
        {
            S_Bowl bowl = new S_Bowl();

            bowl.id = Conversion.SqlToIntOrNull(dataReader["id"]).Value;
            bowl.frameId = Conversion.SqlToIntOrNull(dataReader["frameid"]).Value;
            bowl.bowlNumber = Conversion.SqlToIntOrNull(dataReader["bowlnumber"]).Value;
            bowl.total = Conversion.SqlToIntOrNull(dataReader["total"]);
            bowl.isStrike = Conversion.SqlToBoolOrNull(dataReader["isstrike"]).Value;
            bowl.isSpare = Conversion.SqlToBoolOrNull(dataReader["isspare"]).Value;
            bowl.isSplit = Conversion.SqlToBoolOrNull(dataReader["issplit"]).Value;
            bowl.isGutter = Conversion.SqlToBoolOrNull(dataReader["isgutter"]).Value;
            bowl.isFoul = Conversion.SqlToBoolOrNull(dataReader["isfoul"]).Value;
            bowl.isManuallyModified = Conversion.SqlToBoolOrNull(dataReader["ismanuallymodified"]).Value;
            bowl.pins = Conversion.SqlToString(dataReader["pins"]);
        
            return bowl;
        }

        public static List<S_Bowl> GetBowls()
        {
            List<S_Bowl> bowls = new List<S_Bowl>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM bowl";
                    
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        bowls.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetBowls, Error reading bowl data: {0}", ex.Message));
            }

            return bowls;
        }

        public static List<S_Bowl> GetBowlsByFrameId(long frameid)
        {
            List<S_Bowl> bowls = new List<S_Bowl>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM bowl WHERE frameid=@frameid";
                    command.Parameters.AddWithValue("@frameid", Conversion.LongToSql(frameid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        bowls.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetBowlsByFrameId, Error reading bowl data: {0}", ex.Message));
            }

            return bowls;
        }

        public static S_Bowl GetBowlById(long id)
        {
            S_Bowl bowl = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM bowl WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        bowl = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetBowlById, Error reading bowl data: {0}", ex.Message));
            }

            return bowl;
        }

        public static bool BowlExistById(long id)
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
                    command.CommandText = "SELECT * FROM bowl WHERE id=@id";
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
                logger.Error(string.Format("BowlExistById, Error reading bowl data: {0}", ex.Message));
            }

            return result;
        }

        public static bool BowlExistByFrameId(long frameId)
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
                    command.CommandText = "SELECT * FROM bowl WHERE frameid=@frameid";
                    command.Parameters.AddWithValue("@frameid", Conversion.LongToSql(frameId));

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
                logger.Error(string.Format("BowlExistByFrameId, Error reading bowl data: {0}", ex.Message));
            }

            return result;
        }

        public static bool BowlExistByFrameIdAndBowlNumber(long frameId, int bowlNumber)
        {
            long? bowlId;
            return BowlExistByFrameIdAndBowlNumber(frameId, bowlNumber, out bowlId);
        }

        public static bool BowlExistByFrameIdAndBowlNumber(long frameId, int bowlNumber, out long? bowlId)
        {
            bool result = false;
            bowlId = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    result = BowlExistByFrameIdAndBowlNumber(databaseconnection.getConnection(), frameId, bowlNumber, out bowlId);

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("BowlExistByFrameIdAndBowlNumber, Error reading bowl data: {0}", ex.Message));
            }

            return result;
        }

        public static bool BowlExistByFrameIdAndBowlNumber(MySqlConnection sqlConnection, long frameId, int bowlNumber, out long? bowlId)
        {
            bool result = false;
            bowlId = null;

            try
            {
                //Create Command
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;
                command.CommandText = "SELECT * FROM bowl WHERE frameid=@frameid AND bowlnumber=@bowlnumber";
                command.Parameters.AddWithValue("@frameid", Conversion.LongToSql(frameId));
                command.Parameters.AddWithValue("@bowlnumber", Conversion.IntToSql(bowlNumber));

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = command.ExecuteReader();

                //Check existance
                if (dataReader.HasRows)
                {
                    result = dataReader.HasRows;
                    if (dataReader.Read())
                        bowlId = DataToObject(dataReader).id;
                }

                //close Data Reader
                dataReader.Close();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("BowlExistByFrameIdAndBowlNumber, Error reading bowl data: {0}", ex.Message));
            }

            return result;
        }

        //Insert statement
        public static long? Insert(S_Bowl bowl)
        {
            long? lastInsertedId = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    lastInsertedId = Insert(databaseconnection.getConnection(), bowl);

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting bowl data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        public static long? Insert(MySqlConnection sqlConnection, S_Bowl bowl)
        {
            long? lastInsertedId = null;

            try
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "INSERT INTO bowl ( frameid,  bowlnumber,  total,  isstrike,  isspare,  issplit,  isgutter,  isfoul,  ismanuallymodified, pins) " +
                                                "VALUES (@frameid, @bowlnumber, @total, @isstrike, @isspare, @issplit, @isgutter, @isfoul, @ismanuallymodified, @pins)";

                command.Parameters.AddWithValue("@frameid", Conversion.LongToSql(bowl.frameId));
                command.Parameters.AddWithValue("@bowlnumber", Conversion.IntToSql(bowl.bowlNumber));
                command.Parameters.AddWithValue("@total", Conversion.IntToSql(bowl.total));
                command.Parameters.AddWithValue("@isstrike", Conversion.BoolToSql(bowl.isStrike));
                command.Parameters.AddWithValue("@isspare", Conversion.BoolToSql(bowl.isSpare));
                command.Parameters.AddWithValue("@issplit", Conversion.BoolToSql(bowl.isSplit));
                command.Parameters.AddWithValue("@isgutter", Conversion.BoolToSql(bowl.isGutter));
                command.Parameters.AddWithValue("@isfoul", Conversion.BoolToSql(bowl.isFoul));
                command.Parameters.AddWithValue("@ismanuallymodified", Conversion.BoolToSql(bowl.isManuallyModified));
                command.Parameters.AddWithValue("@pins", Conversion.StringToSql(bowl.pins));

                //Execute command
                command.ExecuteNonQuery();
                lastInsertedId = command.LastInsertedId;

            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting bowl data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Bowl bowl)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    Update(databaseconnection.getConnection(), bowl);

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating bowl data: {0}", ex.Message));
            }
        }

        public static void Update(MySqlConnection sqlConnection, S_Bowl bowl)
        {
            try
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "UPDATE bowl SET frameid=@frameid, bowlnumber=@bowlnumber, total=@total, isstrike=@isstrike, isspare=@isspare, issplit=@issplit, isgutter=@isgutter, isfoul=@isfoul, ismanuallymodified=@ismanuallymodified, pins=@pins WHERE id=@id ";

                command.Parameters.AddWithValue("@id", Conversion.LongToSql(bowl.id));
                command.Parameters.AddWithValue("@frameid", Conversion.LongToSql(bowl.frameId));
                command.Parameters.AddWithValue("@bowlnumber", Conversion.IntToSql(bowl.bowlNumber));
                command.Parameters.AddWithValue("@total", Conversion.IntToSql(bowl.total));
                command.Parameters.AddWithValue("@isstrike", Conversion.BoolToSql(bowl.isStrike));
                command.Parameters.AddWithValue("@isspare", Conversion.BoolToSql(bowl.isSpare));
                command.Parameters.AddWithValue("@issplit", Conversion.BoolToSql(bowl.isSplit));
                command.Parameters.AddWithValue("@isgutter", Conversion.BoolToSql(bowl.isGutter));
                command.Parameters.AddWithValue("@isfoul", Conversion.BoolToSql(bowl.isFoul));
                command.Parameters.AddWithValue("@ismanuallymodified", Conversion.BoolToSql(bowl.isManuallyModified));
                command.Parameters.AddWithValue("@pins", Conversion.StringToSql(bowl.pins));

                //Execute command
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating bowl data: {0}", ex.Message));
            }
        }

        //Delete statement
        public static void Delete(long? id)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                if (databaseconnection.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "DELETE FROM bowl WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting bowl data: {0}", ex.Message));
            }
        }

        public static void Save(MySqlConnection sqlConnection, S_Frame frame, S_Bowl bowl)
        {
            long? bowlId = null;
            bowl.frameId = frame.id;

            if (!BowlManager.BowlExistByFrameIdAndBowlNumber(sqlConnection, frame.id, bowl.bowlNumber, out bowlId))
                bowl.id = Insert(sqlConnection, bowl).Value;
            else
            {
                bowl.id = bowlId.Value;
                // Update total, is... and pins
                Update(sqlConnection, bowl);
            }
        }
    }
}
