using MySql.Data.MySqlClient;
using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Database;
using NLog;
using System;
using System.Collections.Generic;

namespace NBF.Qubica.Managers
{
    public static class FrameManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Frame DataToObject(MySqlDataReader dataReader)
        {
            S_Frame frame = new S_Frame();

            frame.id = Conversion.SqlToIntOrNull(dataReader["id"]).Value;
            frame.gameId = Conversion.SqlToIntOrNull(dataReader["gameid"]).Value;
            frame.frameNumber = Conversion.SqlToIntOrNull(dataReader["framenumber"]).Value;
            frame.progressiveTotal = Conversion.SqlToIntOrNull(dataReader["progressivetotal"]);
            frame.isConvertedSplit = Conversion.SqlToBoolOrNull(dataReader["isconvertedsplit"]).Value;

            return frame;
        }

        public static List<S_Frame> GetFrames()
        {
            List<S_Frame> frames = new List<S_Frame>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM frame";
                    
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        frames.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetFrames, Error reading frame data: {0}", ex.Message));
            }

            return frames;
        }

        public static List<S_Frame> GetFramesByGameid(long gameid)
        {
            List<S_Frame> frames = new List<S_Frame>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM frame WHERE gameid=@gameid";
                    command.Parameters.AddWithValue("@gameid", Conversion.LongToSql(gameid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        frames.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetFramesByGameid, Error reading frame data: {0}", ex.Message));
            }

            return frames;
        }

        public static S_Frame GetFrameById(long id)
        {
            S_Frame frame = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM frame WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        frame = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetFrameById, Error reading frame data: {0}", ex.Message));
            }

            return frame;
        }

        public static bool FrameExistById(long id)
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
                    command.CommandText = "SELECT * FROM frame WHERE id=@id";
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
                logger.Error(string.Format("FrameExistById, Error reading frame data: {0}", ex.Message));
            }

            return result;
        }

        public static bool FrameExistByGameId(long gameId)
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
                    command.CommandText = "SELECT * FROM frame WHERE gameid=@gameid";
                    command.Parameters.AddWithValue("@gameid", Conversion.LongToSql(gameId));

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
                logger.Error(string.Format("FrameExistByGameId, Error reading frame data: {0}", ex.Message));
            }

            return result;
        }

        public static bool FrameExistByGameIdAndFrameNumber(long gameId, int frameNumber)
        {
            long? frameId = null;

            return FrameExistByGameIdAndFrameNumber(gameId, frameNumber, out frameId);
        }

        public static bool FrameExistByGameIdAndFrameNumber(long gameId, int frameNumber, out long? frameId)
        {
            bool result = false;
            frameId = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    result = FrameExistByGameIdAndFrameNumber(databaseconnection.getConnection(), gameId, frameNumber, out frameId);

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("FrameExistByGameIdAndFrameNumber, Error reading frame data: {0}", ex.Message));
            }

            return result;
        }

        public static bool FrameExistByGameIdAndFrameNumber(MySqlConnection sqlConnection, long gameId, int frameNumber, out long? frameId)
        {
            bool result = false;
            frameId = null;

            try
            {
                //Create Command
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;
                command.CommandText = "SELECT * FROM frame WHERE gameid=@gameid AND framenumber=@framenumber";
                command.Parameters.AddWithValue("@gameid", Conversion.LongToSql(gameId));
                command.Parameters.AddWithValue("@framenumber", Conversion.IntToSql(frameNumber));

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = command.ExecuteReader();

                //Check existance
                if (dataReader.HasRows)
                {
                    result = dataReader.HasRows;
                    if (dataReader.Read())
                        frameId = DataToObject(dataReader).id;
                }

                //close Data Reader
                dataReader.Close();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("FrameExistByGameIdAndFrameNumber, Error reading frame data: {0}", ex.Message));
            }

            return result;
        }

        //Insert statement
        public static long? Insert(S_Frame frame)
        {
            long? lastInsertedId = null;
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    lastInsertedId = Insert(databaseconnection.getConnection(), frame);

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting frame data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        public static long? Insert(MySqlConnection sqlConnection, S_Frame frame)
        {
            long? lastInsertedId = null;
            try
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "INSERT INTO frame ( gameid,  framenumber,  progressivetotal,  isconvertedsplit) " +
                                                    "VALUES (@gameid, @framenumber, @progressivetotal, @isconvertedsplit)";

                command.Parameters.AddWithValue("@gameid", Conversion.LongToSql(frame.gameId));
                command.Parameters.AddWithValue("@framenumber", Conversion.IntToSql(frame.frameNumber));
                command.Parameters.AddWithValue("@progressivetotal", Conversion.IntToSql(frame.progressiveTotal));
                command.Parameters.AddWithValue("@isconvertedsplit", Conversion.BoolToSql(frame.isConvertedSplit));

                //Execute command
                command.ExecuteNonQuery();
                lastInsertedId = command.LastInsertedId;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting frame data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Frame frame)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    Update(databaseconnection.getConnection(), frame);

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating frame data: {0}", ex.Message));
            }
        }

        public static void Update(MySqlConnection sqlConnection, S_Frame frame)
        {
            try
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "UPDATE frame SET gameid=@gameid, framenumber=@framenumber, progressivetotal=@progressivetotal, isconvertedsplit=@isconvertedsplit WHERE id=@id ";

                command.Parameters.AddWithValue("@id", Conversion.LongToSql(frame.id));
                command.Parameters.AddWithValue("@gameid", Conversion.LongToSql(frame.gameId));
                command.Parameters.AddWithValue("@framenumber", Conversion.IntToSql(frame.frameNumber));
                command.Parameters.AddWithValue("@progressivetotal", Conversion.IntToSql(frame.progressiveTotal));
                command.Parameters.AddWithValue("@isconvertedsplit", Conversion.BoolToSql(frame.isConvertedSplit));

                //Execute command
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating frame data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM frame WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting frame data: {0}", ex.Message));
            }
        }

        public static void Save(MySqlConnection sqlConnection, S_Game game, S_Frame frame)
        {
            long? frameId = null;
            frame.gameId = game.id;

            if (!FrameManager.FrameExistByGameIdAndFrameNumber(sqlConnection, game.id, frame.frameNumber, out frameId))
                frame.id = Insert(sqlConnection, frame).Value;
            else
            {
                frame.id = frameId.Value;
                // update progressiveTotal and isConvertedsplit
                Update(sqlConnection, frame);
            }

            if (frame.bowl1 != null)
            {
                frame.bowl1.bowlNumber = 1;
                BowlManager.Save(sqlConnection, frame, frame.bowl1);
            }

            if (frame.bowl2 != null)
            {
                frame.bowl2.bowlNumber = 2;
                BowlManager.Save(sqlConnection, frame, frame.bowl2);
            }
            
            if (frame.bowl3 != null)
            {
                frame.bowl3.bowlNumber = 3;
                BowlManager.Save(sqlConnection, frame, frame.bowl3);
            }
        }
    }
}
