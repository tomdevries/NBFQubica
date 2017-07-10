﻿using System;
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
    public static class CompetitionManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Competition DataToCompetitionObject(MySqlDataReader dataReader)
        {
            S_Competition competition = new S_Competition();

            competition.id = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
            competition.challengeid = (long)Conversion.SqlToLongOrNull(dataReader["challengeid"]);
            competition.description = Conversion.SqlToString(dataReader["description"]);
            competition.enddate = (DateTime)Conversion.SqlToDateTimeOrNull(dataReader["enddate"]);
            competition.price = Conversion.SqlToString(dataReader["price"]);
            competition.startdate = (DateTime)Conversion.SqlToDateTimeOrNull(dataReader["startdate"]);

            return competition;
        }

        private static S_CompetitonBowlingcenter DataToCompetitionBowlingcenterObject(MySqlDataReader dataReader)
        {
            S_CompetitonBowlingcenter competition = new S_CompetitonBowlingcenter();

            competition.id = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
            competition.competitionid = (long)Conversion.SqlToLongOrNull(dataReader["competitionid"]);
            competition.bowlingcenterid = (long)Conversion.SqlToLongOrNull(dataReader["bowlingcenterid"]);

            return competition;
        }

        private static S_CompetitionPlayers DataToCompetitionPlayerObject(MySqlDataReader dataReader)
        {
            S_CompetitionPlayers competitionPlayer = new S_CompetitionPlayers();

            competitionPlayer.id = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
            competitionPlayer.competitionid = (long)Conversion.SqlToLongOrNull(dataReader["competitionid"]);
            competitionPlayer.userid = (long)Conversion.SqlToLongOrNull(dataReader["userid"]);

            return competitionPlayer;
        }

        private static S_CompetitionPlayersRanking DataToCompetitionPlayersRankingObject(int rank, MySqlDataReader dataReader)
        {
            S_CompetitionPlayersRanking competitionPlayersRanking = new S_CompetitionPlayersRanking();

            competitionPlayersRanking.Rank = rank;
            competitionPlayersRanking.UserId = (long)Conversion.SqlToLongOrNull(dataReader["id"]);
            competitionPlayersRanking.Name = Conversion.SqlToString(dataReader["playername"]);
            competitionPlayersRanking.FrequentBowlernumber = (long)Conversion.SqlToLongOrNull(dataReader["freeentrycode"]);
            competitionPlayersRanking.Score = (long)Conversion.SqlToLongOrNull(dataReader[3]);

            return competitionPlayersRanking;
        }

        private static S_Player DataToPlayerObject(MySqlDataReader dataReader)
        {
            S_Player player = new S_Player();

            player.userid = Conversion.SqlToLongOrNull(dataReader["id"]).Value;

            return player;
        }

        public static S_CompetitionPlayers GetCompetitionPlayer(long id)
        {
            S_CompetitionPlayers competitionplayer = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM competitionplayers WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        competitionplayer = DataToCompetitionPlayerObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetCompetitionPlayer, Error reading competitionplayersranking data: {0}", ex.Message));
            }

            return competitionplayer;
        }

        public static List<S_CompetitionPlayersRanking> GetCompetitionPlayersRanking(long challangeid, long competitionid, List<S_CompetitionPlayers> playerList, DateTime startdate, DateTime enddate)
        {
            List<S_CompetitionPlayersRanking> competitionplayersranking = new List<S_CompetitionPlayersRanking>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    switch (challangeid)
                    {
                        case 1:
                            command.CommandText = "SELECT d.id " +
                                                  ",      playername " +
                                                  ",      freeentrycode " +
                                                  ",      count(1) AS aantal_strikes " +
                                                  "FROM nbf.game g " +
                                                  ",    nbf.frame f " +
                                                  ",    nbf.bowl b " +
                                                  ",    nbf.event e " +
                                                  ",    nbf.scores s " +
                                                  ", (SELECT user.id " +
                                                  "   ,      user.name " +
                                                  "   ,      user.frequentbowlernumber " +
                                                  "   FROM competitionplayers " +
                                                  "   ,    user " +
                                                  "   WHERE user.id = competitionplayers.userid " +
                                                  "   AND   competitionplayers.competitionid=@competitionid) d " +
                                                  "WHERE b.frameid = f.id " +
                                                  "AND   f.gameid = g.id " + 
                                                  "AND   b.isstrike IS true " +
                                                  "AND   g.freeentrycode IS NOT null " +
                                                  "AND   date(g.startdatetime)>=@startdate " +
                                                  "AND   date(g.startdatetime)<=@enddate " +
                                                  "AND   d.name = g.playername " +
                                                  "AND   d.frequentbowlernumber = g.freeentrycode " +
                                                  "AND   g.eventid = e.id " +
                                                  "AND   e.scoresid = s.id " +
                                                  "AND   s.bowlingcenterid in (SELECT bowlingcenterid FROM competitionbowlingcenter " +
                                                  "                            WHERE  competitionbowlingcenter.competitionid = @competitionid) " +
                                                  "GROUP BY d.id, g.playername, g.freeentrycode " +
                                                  "ORDER BY aantal_strikes DESC ";
                                                  //"limit 10";
                            command.Parameters.AddWithValue("@competitionid", Conversion.LongToSql(competitionid));
                            command.Parameters.AddWithValue("@startdate", Conversion.DateTimeToSql(startdate));
                            command.Parameters.AddWithValue("@enddate", Conversion.DateTimeToSql(enddate));
                            break;
                        case 2:
                            command.CommandText = "SELECT d.id " +
                                                  ",      playername " +
                                                  ",      freeentrycode " +
                                                  ",      count(1) AS aantal_strikes " +
                                                  "FROM nbf.game g " +
                                                  ",    nbf.frame f " +
                                                  ",    nbf.bowl b " +
                                                  ",    nbf.event e " +
                                                  ",    nbf.scores s " +
                                                  ", (SELECT user.id " +
                                                  "   ,      user.name " +
                                                  "   ,      user.frequentbowlernumber " +
                                                  "   FROM competitionplayers " +
                                                  "   ,    user " +
                                                  "   WHERE user.id = competitionplayers.userid " +
                                                  "   AND   competitionplayers.competitionid=@competitionid) d " +
                                                  "WHERE b.frameid = f.id " +
                                                  "AND   f.gameid = g.id " + 
                                                  "AND   b.isspare IS true " +
                                                  "AND   g.freeentrycode IS NOT null " +
                                                  "AND   date(g.startdatetime)>=@startdate " +
                                                  "AND   date(g.startdatetime)<=@enddate " +
                                                  "AND   d.name = g.playername " +
                                                  "AND   d.frequentbowlernumber = g.freeentrycode " +
                                                  "AND   g.eventid = e.id " +
                                                  "AND   e.scoresid = s.id " +
                                                  "AND   s.bowlingcenterid in (SELECT bowlingcenterid FROM competitionbowlingcenter " +
                                                  "                            WHERE  competitionbowlingcenter.competitionid = @competitionid) " +
                                                  "GROUP BY d.id, g.playername, g.freeentrycode " +
                                                  "ORDER BY aantal_strikes DESC ";
                                                  //"limit 10";
                            command.Parameters.AddWithValue("@competitionid", Conversion.LongToSql(competitionid));
                            command.Parameters.AddWithValue("@startdate", Conversion.DateTimeToSql(startdate));
                            command.Parameters.AddWithValue("@enddate", Conversion.DateTimeToSql(enddate));                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        case 6:
                            break;
                        case 7:
                            break;
                        case 8:
                            break;
                        case 9:
                            break;
                        case 10:
                            break;
                    }

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    int rank = 1;
                    while (dataReader.Read())
                    {
                        S_CompetitionPlayersRanking cpr = DataToCompetitionPlayersRankingObject(rank++, dataReader);
                        competitionplayersranking.Add(cpr);
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetCompetitionPlayersRanking, Error reading competitionplayersranking data: {0}", ex.Message));
            }

            return competitionplayersranking;
        }


        public static S_Competition GetCompetition(long id)
        {
            S_Competition competition = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM competition WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        competition = DataToCompetitionObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetCompetition, Error reading competition data: {0}", ex.Message));
            }

            return competition;
        }

        public static List<S_Competition> GetCompetitionsByChallengeId(long id)
        {
            List<S_Competition> competitions = new List<S_Competition>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM competition WHERE challengeid=@challengeid";
                    command.Parameters.AddWithValue("@challengeid", Conversion.LongToSql(id));
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        competitions.Add(DataToCompetitionObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetCompetitionsByChallengeId, Error reading competition data: {0}", ex.Message));
            }

            return competitions;
        }

        public static List<S_CompetitonBowlingcenter> GetBowlingcentersByCompetition(long id)
        {
            List<S_CompetitonBowlingcenter> competitionbowlingcenters = new List<S_CompetitonBowlingcenter>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM competitionbowlingcenter WHERE competitionid=@competitionid";
                    command.Parameters.AddWithValue("@competitionid", Conversion.LongToSql(id));
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        competitionbowlingcenters.Add(DataToCompetitionBowlingcenterObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetBowlingcentersByCompetition, Error reading competitionbowlingcenter data: {0}", ex.Message));
            }

            return competitionbowlingcenters;
        }

        public static List<S_CompetitionPlayers> GetPlayersByCompetition(long id)
        {
            List<S_CompetitionPlayers> competitonplayers = new List<S_CompetitionPlayers>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM competitionplayers WHERE competitionid=@competitionid";
                    command.Parameters.AddWithValue("@competitionid", Conversion.LongToSql(id));
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        competitonplayers.Add(DataToCompetitionPlayerObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetPlayersByCompetition, Error reading competitionplayersranking data: {0}", ex.Message));
            }

            return competitonplayers;
        }

        public static List<S_Player> GetPlayersNotInCompetition(long id)
        {
            List<S_Player> players = new List<S_Player>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT id FROM user where id not in (select userid from competitionplayers where competitionid=@competitionid)";
                    command.Parameters.AddWithValue("@competitionid", Conversion.LongToSql(id));
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        players.Add(DataToPlayerObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetPlayersNotInCompetition, Error reading player data: {0}", ex.Message));
            }

            return players;
        }

        public static List<S_Player> GetPlayersNotInCompetitionByName(long id, string name)
        {
            List<S_Player> players = new List<S_Player>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT id FROM user WHERE name LIKE '%"+name+"%' and id not in (select userid from competitionplayers where competitionid=@competitionid)";
                    command.Parameters.AddWithValue("@competitionid", Conversion.LongToSql(id));
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        players.Add(DataToPlayerObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetPlayersNotInCompetition, Error reading player data: {0}", ex.Message));
            }

            return players;
        }

        public static long? InsertBowlingcenterForCompetition(long competitionid, long bowlingcenterid)
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
                    command.CommandText = "INSERT INTO competitionbowlingcenter (competitionid, bowlingcenterid) VALUES (@competitionid, @bowlingcenterid)";
                    command.Parameters.AddWithValue("@competitionid", Conversion.LongToSql(competitionid));
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingcenterid));

                    //Execute command
                    command.ExecuteNonQuery();
                    lastInsertedId = command.LastInsertedId;

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting bowlingcentercompetition data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        public static void DeleteAllBowlingCentersByCompetition(long id)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                if (databaseconnection.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "DELETE FROM competitionbowlingcenter WHERE competitionid=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting competitionbowlingcenter data: {0}", ex.Message));
            }
        }

        public static void DeletePlayer(long id)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                if (databaseconnection.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "DELETE FROM competitionplayers WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting competitionplayersranking data: {0}", ex.Message));
            }
        }

        public static void AddPlayer(long id, long cid)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                if (databaseconnection.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "INSERT INTO competitionplayers SET userid=@id, competitionid=@competitionid";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));
                    command.Parameters.AddWithValue("@competitionid", Conversion.LongToSql(cid));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("AddPlayer, Error adding competitionplayersranking data: {0}", ex.Message));
            }
        }

        //Insert statement
        public static long? Insert(S_Competition competition)
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
                    command.CommandText = "INSERT INTO competition (challengeid, startdate, enddate, description, price) VALUES (@challengeid, @startdate, @enddate, @description, @price)";
                    command.Parameters.AddWithValue("@challengeid", Conversion.LongToSql(competition.challengeid));
                    command.Parameters.AddWithValue("@startdate", Conversion.DateTimeToSql(competition.startdate));
                    command.Parameters.AddWithValue("@enddate", Conversion.DateTimeToSql(competition.enddate));
                    command.Parameters.AddWithValue("@description", Conversion.StringToSql(competition.description));
                    command.Parameters.AddWithValue("@price", Conversion.StringToSql(competition.price));

                    //Execute command
                    command.ExecuteNonQuery();
                    lastInsertedId = command.LastInsertedId;

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting uUser data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Competition competition)
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

                    command.CommandText = "UPDATE competition SET id=@id, challengeid=@challengeid, startdate=@startdate, enddate=@enddate, description=@description, price=@price WHERE id=@id ";

                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(competition.id));
                    command.Parameters.AddWithValue("@challengeid", Conversion.LongToSql(competition.challengeid));
                    command.Parameters.AddWithValue("@startdate", Conversion.DateTimeToSql(competition.startdate));
                    command.Parameters.AddWithValue("@enddate", Conversion.DateTimeToSql(competition.enddate));
                    command.Parameters.AddWithValue("@description", Conversion.StringToSql(competition.description));
                    command.Parameters.AddWithValue("@price", Conversion.StringToSql(competition.price));

                    //Execute command
                    command.ExecuteNonQuery();

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating competition data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM competition WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting competition data: {0}", ex.Message));
            }
        }
    }
}
