using MySql.Data.MySqlClient;
using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Database;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBF.Qubica.Managers
{
    public static class ProfileManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_BowlScore DataToObject(MySqlDataReader dataReader)
        {
            S_BowlScore bowlScore = new S_BowlScore();

            bowlScore.gamecode = Conversion.SqlToIntOrNull(dataReader["gamecode"]).Value;
            bowlScore.framenumber = Conversion.SqlToIntOrNull(dataReader["framenumber"]).Value;
            bowlScore.progressivetotal = Conversion.SqlToIntOrNull(dataReader["progressivetotal"]).Value;
            bowlScore.bowlnumber = Conversion.SqlToIntOrNull(dataReader["bowlnumber"]).Value;
            bowlScore.total = Conversion.SqlToIntOrNull(dataReader["total"]).Value;
            bowlScore.isStrike = Conversion.SqlToBool(dataReader["isStrike"]);
            bowlScore.isSpare = Conversion.SqlToBool(dataReader["isSpare"]);

            return bowlScore;
        }

        public static List<KeyValuePair<long, string>> getBowlingCentersByUser(string idname, long idnumber)
        {
            List<KeyValuePair<long, string>> bowlingcenternames = new List<KeyValuePair<long,string>>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT bowlingcenter.id, bowlingcenter.name " +
                        "FROM game, event, scores, bowlingcenter " +
                        "WHERE bowlingcenter.id = scores.bowlingcenterid " +
                        " AND event.scoresid = scores.id " +
                        " AND game.eventid = event.id " + 
                        " AND playername = @playername " + 
                        " AND freeentrycode = @freeentrycode" +
                        " GROUP BY bowlingcenter.id";
                    command.Parameters.AddWithValue("@playername", Conversion.StringToSql(idname));
                    command.Parameters.AddWithValue("@freeentrycode", Conversion.LongToSql(idnumber));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        bowlingcenternames.Add(new KeyValuePair<long,string>((long)Conversion.SqlToLongOrNull(dataReader["id"]),Conversion.SqlToString(dataReader["name"])));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("getBowlingCentersByUser, Error reading data: {0}", ex.Message));
            }

            return bowlingcenternames;
        }

        public static List<KeyValuePair<DateTime, KeyValuePair<int,int>>> getScroresPerLanePerDate(long bowlingcenterid, string idname, long idnumber)
        {
            List<KeyValuePair<DateTime, KeyValuePair<int,int>>> scroresperlaneperdate = new List<KeyValuePair<DateTime, KeyValuePair<int,int>>>();
            
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT DATE(game.startdatetime) startdatetime, game.lanenumber, MAX(total) maximum " +
                        " FROM game, event, scores, bowlingcenter " +
                        " WHERE bowlingcenter.id = scores.bowlingcenterid  " +
                        " AND event.scoresid = scores.id  " +
                        " AND game.eventid = event.id  " +
                        " AND playername = @playername  " +
                        " AND freeentrycode = @freeentrycode " +
                        " AND bowlingcenter.id= @bowlingcenterid " +
                        " GROUP BY DATE(startdatetime), lanenumber ";
                    command.Parameters.AddWithValue("@playername", Conversion.StringToSql(idname));
                    command.Parameters.AddWithValue("@freeentrycode", Conversion.LongToSql(idnumber));
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingcenterid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        scroresperlaneperdate.Add(new KeyValuePair<DateTime,KeyValuePair<int,int>>((DateTime)Conversion.SqlToDateTimeOrNull(dataReader["startdatetime"]), new KeyValuePair<int, int>((int)Conversion.SqlToIntOrNull(dataReader["lanenumber"]), (int)Conversion.SqlToIntOrNull(dataReader["maximum"]))));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("getScroresPerLanePerDate, Error reading data: {0}", ex.Message));
            }

            return scroresperlaneperdate;
        }

        public static List<S_BowlScore> getGamesScores(long bowlingcenterid, string idname, long idnumber, DateTime startdatetime, int lanenumber)
        {
            List<S_BowlScore> bowlscores = new List<S_BowlScore>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT game.gamecode, frame.framenumber, frame.progressivetotal, bowl.bowlNumber, bowl.total, bowl.isstrike, bowl.isspare " +
                        " FROM game, event, scores, bowlingcenter, frame, bowl " +
                        " WHERE bowlingcenter.id = scores.bowlingcenterid  " +
                        " AND event.scoresid = scores.id  " +
                        " AND game.eventid = event.id  " +
                        " AND playername = @playername" +
                        " AND freeentrycode = @freeentrycode" +
                        " AND bowlingcenter.id= @bowlingcenterid " +
                        " AND frame.gameid = game.id " +
                        " AND bowl.frameid = frame.id " +
                        " AND DATE(game.startdatetime) = @startdatetime " +
                        " AND game.lanenumber = @lanenumber ";
                    command.Parameters.AddWithValue("@playername", Conversion.StringToSql(idname));
                    command.Parameters.AddWithValue("@freeentrycode", Conversion.LongToSql(idnumber));
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingcenterid));
                    command.Parameters.AddWithValue("@startdatetime", Conversion.DateTimeToSql(startdatetime));
                    command.Parameters.AddWithValue("@lanenumber", Conversion.LongToSql(lanenumber));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        bowlscores.Add(DataToObject(dataReader));
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("getGamesScores, Error reading data: {0}", ex.Message));
            }

            return bowlscores;
        }
    }
}
