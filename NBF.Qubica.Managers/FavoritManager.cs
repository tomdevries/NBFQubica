using MySql.Data.MySqlClient;
using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Database;
using NLog;
using System;
using System.Collections.Generic;

namespace NBF.Qubica.Managers
{
    public static class FavoritManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Favorit DataToObject(MySqlDataReader dataReader)
        {
            S_Favorit score = new S_Favorit();

            score.id = Conversion.SqlToIntOrNull(dataReader["id"]).Value;
            score.userId = Conversion.SqlToIntOrNull(dataReader["userid"]).Value;
            score.favorituserId = Conversion.SqlToIntOrNull(dataReader["favorituserid"]).Value;

            return score;
        }

        public static List<S_Favorit> GetFavoritsByUserId(long userid)
        {
            List<S_Favorit> favorits = new List<S_Favorit>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM favorit WHERE userid = @userid";
                    command.Parameters.AddWithValue("@userid", Conversion.LongToSql(userid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        favorits.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading scores data: {0}", ex.Message));
            }

            return favorits;
        }

        public static long? GetFavoritIdByUserIdFavoritId(long userid, long favorituserid)
        {
            long? id = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM favorit WHERE userid = @userid AND favorituserid = @favorituserid";
                    command.Parameters.AddWithValue("@userid", Conversion.LongToSql(userid));
                    command.Parameters.AddWithValue("@favorituserid", Conversion.LongToSql(favorituserid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        id = (DataToObject(dataReader)).id;

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading scores data: {0}", ex.Message));
            }

            return id;
        }

        public static bool IsUserFavorit(long userid)
        {
            bool isfavorit = false;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM favorit WHERE favorituserid = @favorituserid LIMIT 1";
                    command.Parameters.AddWithValue("@favorituserid", Conversion.LongToSql(userid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        isfavorit = true;

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error checking isfavorit: {0}", ex.Message));
            }

            return isfavorit;
        }

        public static void DeleteFavoritsByUserId(long userid)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                if (databaseconnection.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "DELETE FROM favorit WHERE userid=@userid ";
                    command.Parameters.AddWithValue("@userid", Conversion.LongToSql(userid));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting DeleteCompetitionPlayer data: {0}", ex.Message));
            }
        }

        public static bool IsUserFavoritOfUser(long userid, long favorituserid)
        {
            bool isfavorit = false;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM favorit WHERE userid = @userid AND favorituserid = @favorituserid LIMIT 1";
                    command.Parameters.AddWithValue("@userid", Conversion.LongToSql(userid));
                    command.Parameters.AddWithValue("@favorituserid", Conversion.LongToSql(favorituserid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        isfavorit = true;

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error checking isfavorit for contact: {0}", ex.Message));
            }

            return isfavorit;
        }
        //Insert statement
        public static long? Insert(S_Favorit favorit)
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
                    command.CommandText = "INSERT INTO favorit (userid, favorituserid) VALUES (@userid, @favorituserid)";
                    command.Parameters.AddWithValue("@userid", Conversion.LongToSql(favorit.userId));
                    command.Parameters.AddWithValue("@favorituserid", Conversion.LongToSql(favorit.favorituserId));

                    //Execute command
                    command.ExecuteNonQuery();
                    lastInsertedId = command.LastInsertedId;
 
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

        //Update statement
        public static void Update(S_Favorit favorit)
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

                    command.CommandText = "UPDATE favorit SET userid=@userid, favorituserid=@favorituserid WHERE id=@id ";

                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(favorit.id));
                    command.Parameters.AddWithValue("@userid", Conversion.LongToSql(favorit.userId));
                    command.Parameters.AddWithValue("@favorituserid", Conversion.LongToSql(favorit.favorituserId));

                    //Execute command
                    command.ExecuteNonQuery();

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating favorit data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM favorit WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting favorit data: {0}", ex.Message));
            }
        }
    }
}
