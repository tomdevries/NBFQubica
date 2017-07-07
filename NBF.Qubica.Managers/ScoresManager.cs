using MySql.Data.MySqlClient;
using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Database;
using NLog;
using System;
using System.Collections.Generic;

namespace NBF.Qubica.Managers
{
    public static class ScoresManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Scores DataToObject(MySqlDataReader dataReader)
        {
            S_Scores score = new S_Scores();

            score.id = Conversion.SqlToIntOrNull(dataReader["id"]).Value;
            score.bowlingCenterId = Conversion.SqlToIntOrNull(dataReader["bowlingcenterid"]).Value;

            return score;
        }

        public static List<S_Scores> GetScores()
        {
            List<S_Scores> scores = new List<S_Scores>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM scores";
                    
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        scores.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetScores, Error reading scores data: {0}", ex.Message));
            }

            return scores;
        }

        public static S_Scores GetScoreById(long id)
        {
            S_Scores scores = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    scores = GetScoreById(databaseconnection.getConnection(), id);

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetScoreById, Error reading scores data: {0}", ex.Message));
            }

            return scores;
        }

        public static S_Scores GetScoreById(MySqlConnection sqlConnection, long id)
        {
            S_Scores scores = null;

            try
            {
                //Create Command
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;
                command.CommandText = "SELECT * FROM scores WHERE id=@id";
                command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = command.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.Read())
                    scores = DataToObject(dataReader);

                //close Data Reader
                dataReader.Close();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetScoreById, Error reading scores data: {0}", ex.Message));
            }

            return scores;
        }

        public static S_Scores GetScoresByBowlingCenterId(long bowlingCenterId)
        {
            S_Scores scores = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM scores WHERE bowlingcenterid=@bowlingcenterid";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingCenterId));

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
                logger.Error(string.Format("GetScoresByBowlingCenterId, Error reading scores data: {0}", ex.Message));
            }

            return scores;
        }

        public static bool ScoresExistById(long id)
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
                    command.CommandText = "SELECT * FROM scores WHERE id=@id";
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
                logger.Error(string.Format("ScoresExistById, Error reading scores data: {0}", ex.Message));
            }

            return result;
        }

        public static bool ScoresExistByBowlingCenterId(long? bowlingCenterId)
        {
            long? scoresId = null;
            return ScoresExistByBowlingCenterId(bowlingCenterId, out scoresId);
        }

        public static bool ScoresExistByBowlingCenterId(long? bowlingCenterId, out long? scoresId)
        {
            bool result = false;
            scoresId = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM scores WHERE bowlingcenterid=@bowlingcenterid";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingCenterId));
                    
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Check existance
                    if (dataReader.HasRows)
                    {
                        result = dataReader.HasRows;
                        if (dataReader.Read())
                            scoresId = DataToObject(dataReader).id;
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("ScoresExistByBowlingCenterId, Error reading scores data: {0}", ex.Message));
            }

            return result;
        }

        public static bool ScoresExistByBowlingCenterId(MySqlConnection sqlConnection, long? bowlingCenterId, out long? scoresId)
        {
            bool result = false;
            scoresId = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM scores WHERE bowlingcenterid=@bowlingcenterid";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingCenterId));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Check existance
                    if (dataReader.HasRows)
                    {
                        result = dataReader.HasRows;
                        if (dataReader.Read())
                            scoresId = DataToObject(dataReader).id;
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("ScoresExistByBowlingCenterId, Error reading scores data: {0}", ex.Message));
            }

            return result;
        }

        //Insert statement
        public static long? Insert(S_Scores scores)
        {
            long? lastInsertedId=null;
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    lastInsertedId = Insert(databaseconnection.getConnection(), scores);
 
                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting scores data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        public static long? Insert(MySqlConnection sqlConnection, S_Scores scores)
        {
            long? lastInsertedId = null;
            try
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;
                command.CommandText = "INSERT INTO scores (bowlingcenterid) VALUES (@bowlingcenterid)";
                command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(scores.bowlingCenterId));

                //Execute command
                command.ExecuteNonQuery();
                lastInsertedId = command.LastInsertedId;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting scores data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Scores scores)
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

                    command.CommandText = "UPDATE scores SET bowlingcenterid=@bowlingcenterid WHERE id=@id ";

                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(scores.id));
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(scores.bowlingCenterId));

                    //Execute command
                    command.ExecuteNonQuery();

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating scores data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM scores WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting scores data: {0}", ex.Message));
            }
        }

        public static void Save(MySqlConnection sqlConnection, long bowlingCenterId, S_Scores score)
        {
            long? scoreId = null;
            score.bowlingCenterId = bowlingCenterId;

            if (!ScoresManager.ScoresExistByBowlingCenterId(sqlConnection, bowlingCenterId, out scoreId))
                score.id = Insert(sqlConnection, score).Value;
            else
                score.id = ScoresManager.GetScoreById(sqlConnection, scoreId.Value).id;

            foreach (S_Event bowlEvent in score.Events)
            {
                EventManager.Save(sqlConnection, score, bowlEvent);
            }
        }
    }
}
