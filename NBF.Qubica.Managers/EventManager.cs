using MySql.Data.MySqlClient;
using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Database;
using NLog;
using System;
using System.Collections.Generic;

namespace NBF.Qubica.Managers
{
    public static class EventManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Event DataToObject(MySqlDataReader dataReader)
        {
            S_Event bowlEvent = new S_Event();

            bowlEvent.id = Conversion.SqlToIntOrNull(dataReader["id"]).Value;
            bowlEvent.eventCode = Conversion.SqlToIntOrNull(dataReader["eventcode"]).Value;
            bowlEvent.scoresId = Conversion.SqlToIntOrNull(dataReader["scoresid"]).Value;
            bowlEvent.openDateTime = Conversion.SqlToDateTimeOrNull(dataReader["opendatetime"]).Value;
            bowlEvent.closeDateTime = Conversion.SqlToDateTimeOrNull(dataReader["closedatetime"]);
            bowlEvent.status = (Status)Enum.Parse(typeof(Status),Conversion.SqlToString(dataReader["status"]));
            bowlEvent.openMode = (OpenMode)Enum.Parse(typeof(OpenMode),Conversion.SqlToString(dataReader["openmode"]));

            return bowlEvent;
        }

        public static List<S_Event> GetEvents()
        {
            List<S_Event> events = new List<S_Event>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM event";

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        events.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetEvents, Error reading event data: {0}", ex.Message));
            }

            return events;
        }

        public static List<S_Event> GetEventsByScoreId(long scoresId)
        {
            List<S_Event> events = new List<S_Event>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM event WHERE scoresid=@scoresid";
                    command.Parameters.AddWithValue("@scoresId", Conversion.LongToSql(scoresId));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        events.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetEventsByScoreId, Error reading event data: {0}", ex.Message));
            }

            return events;
        }

        public static S_Event GetEventById(long id)
        {
            S_Event bowlEvent = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM event WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        bowlEvent = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetEventById, Error reading event data: {0}", ex.Message));
            }

            return bowlEvent;
        }

        public static bool EventExistByScoreIdAndEventCode(long scoresId, long eventCode)
        {
            long? eventId = null;
            return EventExistByScoreIdAndEventCode(scoresId, eventCode, out eventId);
        }

        public static bool EventExistByScoreIdAndEventCode(long scoresId, long eventCode, out long? eventId)
        {
            bool result = false;
            eventId = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    result = EventExistByScoreIdAndEventCode(databaseconnection.getConnection(), scoresId, eventCode, out eventId);

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("EventExistByScoreIdAndEventCode, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return result;
        }

        public static bool EventExistByScoreIdAndEventCode(MySqlConnection sqlConnection, long scoresId, long eventCode, out long? eventId)
        {
            bool result = false;
            eventId = null;

            try
            {
                //Create Command
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;
                command.CommandText = "SELECT * FROM event WHERE scoresid=@scoresid AND eventcode=@eventcode";
                command.Parameters.AddWithValue("@scoresid", Conversion.LongToSql(scoresId));
                command.Parameters.AddWithValue("@eventCode", Conversion.LongToSql(eventCode));

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = command.ExecuteReader();

                //Check existance
                if (dataReader.HasRows)
                {
                    result = dataReader.HasRows;
                    if (dataReader.Read())
                        eventId = DataToObject(dataReader).id;
                }

                //close Data Reader
                dataReader.Close();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("EventExistByScoreIdAndEventCode, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return result;
        }

        public static bool EventExistById(long id)
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
                    command.CommandText = "SELECT * FROM event WHERE id=@id";
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
                logger.Error(string.Format("EventExistById, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return result;
        }

        public static bool EventExistByScoreId(long scoresId)
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
                    command.CommandText = "SELECT * FROM event WHERE scoresid=@scorseid";
                    command.Parameters.AddWithValue("@scoresid", Conversion.LongToSql(scoresId));

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
                logger.Error(string.Format("EventExistByScoreId, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return result;
        }

        //Insert statement
        public static long? Insert(S_Event bowlEvent)
        {
            long? lastInsertedId = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    lastInsertedId = Insert(databaseconnection.getConnection(),bowlEvent);

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting event data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        public static long? Insert(MySqlConnection sqlConnection, S_Event bowlEvent)
        {
            long? lastInsertedId = null;

            try
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "INSERT INTO event ( eventcode,  scoresid,  opendatetime,  closedatetime,  status,  openmode) " +
                                                    "VALUES (@eventcode, @scoresid, @opendatetime, @closedatetime, @status, @openmode)";

                command.Parameters.AddWithValue("@eventcode", Conversion.LongToSql(bowlEvent.eventCode));
                command.Parameters.AddWithValue("@scoresid", Conversion.LongToSql(bowlEvent.scoresId));
                command.Parameters.AddWithValue("@opendatetime", Conversion.DateTimeToSql(bowlEvent.openDateTime));
                command.Parameters.AddWithValue("@closedatetime", Conversion.DateTimeToSql(bowlEvent.closeDateTime));
                command.Parameters.AddWithValue("@status", Conversion.StringToSql(bowlEvent.status.ToString()));
                command.Parameters.AddWithValue("@openmode", Conversion.StringToSql(bowlEvent.openMode.ToString()));

                //Execute command
                command.ExecuteNonQuery();
                lastInsertedId = command.LastInsertedId;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting event data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Event bowlEvent)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    Update(databaseconnection.getConnection(), bowlEvent);

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating event data: {0}", ex.Message));
            }
        }

        public static void Update(MySqlConnection sqlConnection, S_Event bowlEvent)
        {
            try
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;
                command.CommandText = "UPDATE event SET eventcode=@eventcode, scoresid=@scoresid, opendatetime=@opendatetime, closedatetime=@closedatetime, status=@status, openmode=@openmode WHERE id=@id ";

                command.Parameters.AddWithValue("@id", Conversion.LongToSql(bowlEvent.id));
                command.Parameters.AddWithValue("@eventcode", Conversion.LongToSql(bowlEvent.eventCode));
                command.Parameters.AddWithValue("@scoresid", Conversion.LongToSql(bowlEvent.scoresId));
                command.Parameters.AddWithValue("@opendatetime", Conversion.DateTimeToSql(bowlEvent.openDateTime));
                command.Parameters.AddWithValue("@closedatetime", Conversion.DateTimeToSql(bowlEvent.closeDateTime));
                command.Parameters.AddWithValue("@status", Conversion.StringToSql(bowlEvent.status.ToString()));
                command.Parameters.AddWithValue("@openmode", Conversion.StringToSql(bowlEvent.openMode.ToString()));

                //Execute command
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating event data: {0}", ex.Message));
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
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "DELETE FROM event WHERE id=@id";


                    cmd.Connection = databaseconnection.getConnection();
                    cmd.Parameters.AddWithValue("@id", Conversion.LongToSql(id));
                    cmd.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting event data: {0}", ex.Message));
            }
        }

        public static void Save(MySqlConnection sqlConnection, S_Scores score, S_Event bowlEvent)
        {
            long? eventId = null;
            bowlEvent.scoresId = score.id;

            if (!EventManager.EventExistByScoreIdAndEventCode(sqlConnection, score.id, bowlEvent.eventCode, out eventId))
                bowlEvent.id = Insert(sqlConnection, bowlEvent).Value;
            else
            {
                bowlEvent.id = eventId.Value;
                // closeDateTime and Status might have changed
                Update(sqlConnection, bowlEvent);
            }

            foreach (S_Game game in bowlEvent.games)
            {
                GameManager.Save(sqlConnection, bowlEvent, game);
            }
        }
    }
}
