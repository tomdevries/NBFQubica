using MySql.Data.MySqlClient;
using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Database;
using NLog;
using System;
using System.Collections.Generic;

namespace NBF.Qubica.Managers
{
    public static class BowlingCenterManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_BowlingCenter DataToObject(MySqlDataReader dataReader)
        {
            S_BowlingCenter bowlingCenter = new S_BowlingCenter();

            bowlingCenter.id = Conversion.SqlToIntOrNull(dataReader["id"]).Value;
            bowlingCenter.name = Conversion.SqlToString(dataReader["name"]);
            bowlingCenter.uri = Conversion.SqlToString(dataReader["uri"]);
            bowlingCenter.centerId = Conversion.SqlToIntOrNull(dataReader["centerId"]).Value;
            bowlingCenter.APIversion = Conversion.SqlToString(dataReader["apiversion"]);
            bowlingCenter.numberOfLanes = Conversion.SqlToIntOrNull(dataReader["numberoflanes"]).Value;
            bowlingCenter.lastSyncDate = Conversion.SqlToDateTimeOrNull(dataReader["lastsyncdate"]);
            bowlingCenter.appname = Conversion.SqlToString(dataReader["appname"]);
            bowlingCenter.secretkey = Conversion.SqlToString(dataReader["secretkey"]);


            bowlingCenter.address = Conversion.SqlToString(dataReader["address"]);
            bowlingCenter.city = Conversion.SqlToString(dataReader["city"]);
            bowlingCenter.email = Conversion.SqlToString(dataReader["email"]);
            bowlingCenter.logo = Conversion.SqlToString(dataReader["logo"]);
            bowlingCenter.phonenumber = Conversion.SqlToString(dataReader["phonenumber"]);
            bowlingCenter.website = Conversion.SqlToString(dataReader["website"]);
            bowlingCenter.zipcode = Conversion.SqlToString(dataReader["zipcode"]);

            return bowlingCenter;
        }

        public static List<S_BowlingCenter> GetBowlingCenters()
        {
            List<S_BowlingCenter> bowlingCenters = new List<S_BowlingCenter>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM bowlingcenter";
                    
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        bowlingCenters.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetBowlingCenters, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return bowlingCenters;
        }

        public static List<S_BowlingCenter> GetBowlingCentersByName(string name)
        {
            List<S_BowlingCenter> bowlingCenters = new List<S_BowlingCenter>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM bowlingcenter WHERE email LIKE '%" + name + "%'";

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        bowlingCenters.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetBowlingCenters, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return bowlingCenters;
        }

        public static S_BowlingCenter GetBowlingCenterByName(string name)
        {
            S_BowlingCenter bowlingCenter = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM bowlingcenter WHERE email=@email";
                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(name));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        bowlingCenter = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetBowlingCenter, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return bowlingCenter;
        }

        public static S_BowlingCenter GetBowlingCenterById(long id)
        {
            S_BowlingCenter bowlingCenter = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM bowlingcenter WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        bowlingCenter = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetBowlingCenterById, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return bowlingCenter;
        }

        public static bool BowlingCenterExistById(long id)
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
                    command.CommandText = "SELECT * FROM bowlingcenter WHERE id=@id";
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
                logger.Error(string.Format("BowlingCenterExistById, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return result;
        }

        public static bool BowlingCenterExistByName(string name)
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
                    command.CommandText = "SELECT * FROM bowlingcenter WHERE email=@email";
                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(name));

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
                logger.Error(string.Format("BowlingCenterExistByName, Error reading bowlingcenter data: {0}", ex.Message));
            }

            return result;
        }

        //Insert statement
        public static long? Insert(S_BowlingCenter bowlingCenter)
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

                    command.CommandText = "INSERT INTO bowlingcenter ( email,  uri,  centerId,  apiversion,  numberoflanes,  lastsyncdate, address, city, email, logo, phonenumber, website, zipcode, appname, secretkey) " +
                                                             "VALUES (@email, @uri, @centerId, @apiversion, @numberoflanes, @lastsyncdate, @address, @city, @email, @logo, @phonenumber, @website, @zipcode, @appname, @secretkey)";

                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(bowlingCenter.name));
                    command.Parameters.AddWithValue("@uri", Conversion.StringToSql(bowlingCenter.uri));
                    command.Parameters.AddWithValue("@centerId", Conversion.IntToSql(bowlingCenter.centerId));
                    command.Parameters.AddWithValue("@apiversion", Conversion.StringToSql(bowlingCenter.APIversion));
                    command.Parameters.AddWithValue("@numberoflanes", Conversion.IntToSql(bowlingCenter.numberOfLanes));
                    command.Parameters.AddWithValue("@lastsyncdate", Conversion.DateTimeToSql(bowlingCenter.lastSyncDate));
                    command.Parameters.AddWithValue("@address", Conversion.StringToSql(bowlingCenter.address)); 
                    command.Parameters.AddWithValue("@city", Conversion.StringToSql(bowlingCenter.city)); 
                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(bowlingCenter.email)); 
                    command.Parameters.AddWithValue("@logo", Conversion.StringToSql(bowlingCenter.logo)); 
                    command.Parameters.AddWithValue("@phonenumber", Conversion.StringToSql(bowlingCenter.phonenumber)); 
                    command.Parameters.AddWithValue("@website", Conversion.StringToSql(bowlingCenter.website));
                    command.Parameters.AddWithValue("@zipcode", Conversion.StringToSql(bowlingCenter.zipcode));
                    command.Parameters.AddWithValue("@appname", Conversion.StringToSql(bowlingCenter.appname));
                    command.Parameters.AddWithValue("@secretkey", Conversion.StringToSql(bowlingCenter.secretkey));


                    //Execute command
                    command.ExecuteNonQuery();
                    lastInsertedId = command.LastInsertedId;

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting bowlingcenter data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_BowlingCenter bowlingCenter)
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

                    command.CommandText = "UPDATE bowlingcenter SET email=@email, uri=@uri, centerId=@centerId, apiversion=@apiversion, numberoflanes=@numberoflanes, lastsyncdate=@lastsyncdate, address=@address, city=@city, email=@email , logo=@logo, phonenumber=@phonenumber, website=@website, zipcode=@zipcode, appname=@appname, secretkey=@secretkey WHERE id=@id ";

                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(bowlingCenter.id));
                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(bowlingCenter.name));
                    command.Parameters.AddWithValue("@uri", Conversion.StringToSql(bowlingCenter.uri));
                    command.Parameters.AddWithValue("@centerId", Conversion.IntToSql(bowlingCenter.centerId));
                    command.Parameters.AddWithValue("@apiversion", Conversion.StringToSql(bowlingCenter.APIversion));
                    command.Parameters.AddWithValue("@numberoflanes", Conversion.IntToSql(bowlingCenter.numberOfLanes));
                    command.Parameters.AddWithValue("@lastsyncdate", Conversion.DateTimeToSql(bowlingCenter.lastSyncDate));
                    command.Parameters.AddWithValue("@address", Conversion.StringToSql(bowlingCenter.address));
                    command.Parameters.AddWithValue("@city", Conversion.StringToSql(bowlingCenter.city));
                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(bowlingCenter.email));
                    command.Parameters.AddWithValue("@logo", Conversion.StringToSql(bowlingCenter.logo));
                    command.Parameters.AddWithValue("@phonenumber", Conversion.StringToSql(bowlingCenter.phonenumber));
                    command.Parameters.AddWithValue("@website", Conversion.StringToSql(bowlingCenter.website));
                    command.Parameters.AddWithValue("@zipcode", Conversion.StringToSql(bowlingCenter.zipcode));
                    command.Parameters.AddWithValue("@appname", Conversion.StringToSql(bowlingCenter.appname));
                    command.Parameters.AddWithValue("@secretkey", Conversion.StringToSql(bowlingCenter.secretkey));

                    //Execute command
                    command.ExecuteNonQuery();

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating bowlingcenter data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM bowlingcenter WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting bowlingcenter data: {0}", ex.Message));
                throw new Exception("Er is een fout opgetreden bij het verwijderen");
            }
        }

        public static void Save(S_BowlingCenter bowlingCenter)
        {
            logger.Info("Start saving");

            if (bowlingCenter != null)
            {
                try
                {
                    if (BowlingCenterExistById(bowlingCenter.id))
                    {
                        DatabaseConnection databaseconnection = new DatabaseConnection();

                        //open connection
                        if (databaseconnection.OpenConnection())
                        {
                            MySqlConnection sqlConnection = databaseconnection.getConnection();

                            // save the scores
                            if (bowlingCenter.scores != null)
                                ScoresManager.Save(sqlConnection, bowlingCenter.id, bowlingCenter.scores);
                            else
                                logger.Warn("Er zijn geen scores");

                            //close connection
                            databaseconnection.CloseConnection();
                        }

                        // update the last_syncdate set by the servicehandler
                        Update(bowlingCenter);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("fout bij het opslaan van de scores: "+ ex.Message);
                }
            }

            logger.Info("End saving");
        }
    }
}
