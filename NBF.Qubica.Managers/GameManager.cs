using MySql.Data.MySqlClient;
using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Database;
using NLog;
using System;
using System.Collections.Generic;

namespace NBF.Qubica.Managers
{
    public static class GameManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Game DataToGameObject(MySqlDataReader dataReader)
        {
            S_Game game = new S_Game();

            game.id = Conversion.SqlToIntOrNull(dataReader["id"]).Value;
            game.gameCode = Conversion.SqlToIntOrNull(dataReader["gamecode"]).Value;
            game.eventId = Conversion.SqlToIntOrNull(dataReader["eventid"]).Value;
            game.laneNumber = Conversion.SqlToIntOrNull(dataReader["lanenumber"]).Value;
            game.playerName = Conversion.SqlToString(dataReader["playername"]);
            game.fullName = Conversion.SqlToString(dataReader["fullname"]);
            game.freeEntryCode = Conversion.SqlToString(dataReader["freeentrycode"]);
            game.playerPosition = Conversion.SqlToIntOrNull(dataReader["playerposition"]).Value;
            game.startDateTime = Conversion.SqlToDateTimeOrNull(dataReader["startdatetime"]).Value;
            game.endDateTime = Conversion.SqlToDateTimeOrNull(dataReader["enddatetime"]);
            game.gameNumber = Conversion.SqlToIntOrNull(dataReader["gamenumber"]).Value; ;
            game.handicap = Conversion.SqlToIntOrNull(dataReader["hdcp"]);
            game.total = Conversion.SqlToIntOrNull(dataReader["total"]);

            return game;
        }

        public static List<S_Game> GetGames()
        {
            List<S_Game> bowlingCenters = new List<S_Game>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM game";
                    
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        bowlingCenters.Add(DataToGameObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return bowlingCenters;
        }

        public static List<S_Game> GetGamesByEventId(long eventid)
        {
            List<S_Game> bowlingCenters = new List<S_Game>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM game WHERE eventid=@eventid";
                    command.Parameters.AddWithValue("@eventid", Conversion.LongToSql(eventid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        bowlingCenters.Add(DataToGameObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return bowlingCenters;
        }

        public static S_Game GetGameById(long id)
        {
            S_Game game = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM game WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        game = DataToGameObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return game;
        }

        public static List<S_Game> GetGamesByBowlingIdAndFreeEntryCodeAndOpenDate(long bowlingCenterId, string freeEntryCode, DateTime bowlingDateTime)
        {
            List<S_Game> games = new List<S_Game>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT game.* FROM game, event, scores WHERE game.eventid = event.id AND event.scoresid = scores.id AND scores.bowlingcenterid=@bowlingcenterid AND game.freeentrycode=@freeentrycode AND date(game.startdatetime)=@startdate";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingCenterId));
                    command.Parameters.AddWithValue("@freeentrycode", Conversion.StringToSql(freeEntryCode));
                    command.Parameters.AddWithValue("@startdate", Conversion.DateTimeToSql(bowlingDateTime));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        games.Add(DataToGameObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return games;
        }

        public static Scores GetProfileScores(string username, long frequentbowlernumber)
        {
            Scores scores = new Scores();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT game.id, sum(isstrike) as strikes, sum(isspare) as spares, sum(issplit) as splits, sum(isgutter) as gutters, sum(isfoul) as fouls " +
                                          "FROM game, frame, bowl " +
                                          "WHERE game.id = frame.gameid " +
                                          "AND frame.id = bowl.frameid " +
                                          "AND game.playername = @username " +
                                          "AND game.freeentrycode = @frequentbowlernumber";
                    command.Parameters.AddWithValue("@username", Conversion.StringToSql(username));
                    command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(frequentbowlernumber));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                    {
                        scores.faults = Conversion.SqlToIntOrNull(dataReader["fouls"]).Value;
                        scores.gutters = Conversion.SqlToIntOrNull(dataReader["gutters"]).Value;
                        scores.spares = Conversion.SqlToIntOrNull(dataReader["spares"]).Value;
                        scores.splits = Conversion.SqlToIntOrNull(dataReader["splits"]).Value;
                        scores.strikes = Conversion.SqlToIntOrNull(dataReader["strikes"]).Value;
                    }
                    
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return scores;
        }
        
        public static GameScores GetScoresByUser(S_User user)
        {
            GameScores gameScores = new GameScores();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT sum(w.isstrike) as strikes, sum(w.isspare) as spares, max(g.total) as bestgame " +
                                          "FROM game as g, " +
                                          "	    frame as f, " +
                                          "	    bowl as w " +
                                          "WHERE g.id = f.gameid " +
                                          "  AND f.id = w.frameid " +
                                          "  AND g.playername = @username " +
                                          "  AND g.freeentrycode = @frequentbowlernumber " +
                    command.Parameters.AddWithValue("@username", Conversion.StringToSql(user.username));
                    command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(user.frequentbowlernumber));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                    {
                        gameScores.strikes = Conversion.SqlToIntOrNull(dataReader["strikes"]).Value;
                        gameScores.spares = Conversion.SqlToIntOrNull(dataReader["spares"]).Value;
                        gameScores.bestGame = Conversion.SqlToIntOrNull(dataReader["bestgame"]).Value;
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return gameScores;
        }

        public static SearchUser[] SearchUser(string searchstring)
        {
            List<SearchUser> sul = new List<SearchUser>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT g.playername, g.freeentrycode, g.fullname, sum(w.isstrike) as strikes, sum(w.isspare) as spares, max(g.total) as bestgame, u.id " +
                                          "FROM game as g, " + 
                                          "	    frame as f, " + 
                                          "	    bowl as w, " +
	                                      "     user as u " + 
                                          "WHERE g.id = f.gameid " + 
                                          "AND f.id = w.frameid " +
                                          "AND u.username = g.playername " +
                                          "AND g.playername like @searchstring " +
                                          "GROUP BY playername, freeentrycode, fullname ";
                    command.Parameters.AddWithValue("@searchstring", "%" + Conversion.StringToSql(searchstring) + "%");

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        SearchUser su = new Classes.SearchUser();
                        su.scores = new GameScores();
                        su.scores.strikes = Conversion.SqlToIntOrNull(dataReader["strikes"]).Value;
                        su.scores.spares = Conversion.SqlToIntOrNull(dataReader["spares"]).Value;
                        su.scores.bestGame = Conversion.SqlToIntOrNull(dataReader["bestgame"]).Value;
                        su.userid = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
                        su.name = Conversion.SqlToString(dataReader["playername"]);
                        su.fullname = Conversion.SqlToString(dataReader["fullname"]);
                        if (!string.IsNullOrEmpty(Conversion.SqlToString(dataReader["freeentrycode"])))
                            su.freqentbowlernumber = Conversion.SqlToLongOrNull(dataReader["freeentrycode"]).Value;

                        sul.Add(su);
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return sul.ToArray();
        }

        public static UserGames GetGamesByUser(S_User user)
        {
            UserGames userGames = new UserGames();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT b.id, b.email, DATE(g.startdatetime) as date, sum(w.isstrike) as strikes, sum(w.isspare) as spares, max(g.total) as bestgame " +
                                          "FROM bowlingcenter as b, " +
                                          "	    scores as s,  " +
                                          "     event as e, " +
                                          "     game as g, " +
                                          "	    frame as f, " +
                                          "	    bowl as w " +
                                          "WHERE b.id = s.bowlingcenterid " +
                                          "  AND s.id = e.scoresid " +
                                          "  AND e.id = g.eventid " +
                                          "  AND g.id = f.gameid " +
                                          "  AND f.id = w.frameid " +
                                          "  AND g.playername = @username " +
                                          "  AND g.freeentrycode = @frequentbowlernumber " +
                                          "GROUP BY b.id, b.email, DATE(g.startdatetime) " +
                                          "ORDER BY date DESC";
                    command.Parameters.AddWithValue("@username", Conversion.StringToSql(user.username));
                    command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(user.frequentbowlernumber));
                    logger.Debug(command.CommandText);
                    logger.Debug(user.username);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    userGames.userid = user.id;
                    userGames.name = user.username;
                    
                    List<Games> gamesList = new List<Games>();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        Games games = new Games();
                        games.bowlingcenterid = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
                        games.bowlingcenterName = Conversion.SqlToString(dataReader["email"]);
                        games.bowlDate = Conversion.SqlToDateTimeOrNull(dataReader["date"]).Value;
                        games.scores = new GameScores();
                        games.scores.strikes = Conversion.SqlToIntOrNull(dataReader["strikes"]).Value;
                        games.scores.spares = Conversion.SqlToIntOrNull(dataReader["spares"]).Value;
                        games.scores.bestGame = Conversion.SqlToIntOrNull(dataReader["bestgame"]).Value;

                        gamesList.Add(games);
                    }

                    userGames.games = gamesList.ToArray();

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return userGames;
        }

        public static PlayedGames GetPlayedGamesByUserAndBowlingcenterAndDate(S_User user, S_BowlingCenter bowlingcenter, DateTime gamedate)
        {
            PlayedGames playedGames = new PlayedGames();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT b.id, b.email, DATE(g.startdatetime) as date, g.total, sum(w.isstrike) as strikes, sum(w.isspare) as spares, g.id as gameid, g.gamecode " +
                                          "FROM bowlingcenter as b, " +
                                          "	    scores as s,  " +
                                          "     event as e, " +
                                          "     game as g, " +
                                          "	    frame as f, " +
                                          "	    bowl as w " +
                                          "WHERE b.id = @bowlingcenterid " +
                                          "AND b.id = s.bowlingcenterid " +
                                          "AND s.id = e.scoresid " +
                                          "AND e.id = g.eventid " +
                                          "AND g.id = f.gameid " +
                                          "AND f.id = w.frameid " +
                                          "AND g.playername = @username " +
                                          "AND g.freeentrycode = @frequentbowlernumber " +
                                          "AND DATE(g.startdatetime) = @gamedate " +
                                          "GROUP BY b.id, b.email, DATE(g.startdatetime), g.id " +
                                          "ORDER BY b.email, date DESC";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingcenter.id));
                    command.Parameters.AddWithValue("@username", Conversion.StringToSql(user.username));
                    command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(user.frequentbowlernumber));
                    command.Parameters.AddWithValue("@gamedate", Conversion.DateTimeToSql(gamedate));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    playedGames.bowlingcenterid = bowlingcenter.id;
                    playedGames.bowlingcenterName = bowlingcenter.name;
                    playedGames.date_played = gamedate;

                    List<GamesPlayed> gamesPlayedList = new List<GamesPlayed>();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        GamesPlayed gamesplayed = new GamesPlayed();
                        gamesplayed.gameid = Conversion.SqlToLongOrNull(dataReader["gameid"]).Value;
                        gamesplayed.gameName = Conversion.SqlToString(dataReader["gamecode"]);
                        gamesplayed.score = new Score();

                        gamesplayed.score.totalScore = Conversion.SqlToIntOrNull(dataReader["total"]).Value;
                        gamesplayed.score.strikes = Conversion.SqlToIntOrNull(dataReader["strikes"]).Value;
                        gamesplayed.score.spares = Conversion.SqlToIntOrNull(dataReader["spares"]).Value;

                        gamesPlayedList.Add(gamesplayed);
                    }

                    playedGames.games = gamesPlayedList.ToArray();

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return playedGames;
        }

        public static Game GetPlayedGameById(long gameid)
        {
            Game game = new Game();
            logger.Debug("GAMEID " + gameid);
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    logger.Debug("Opened " + gameid);
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "select b.id as bowlingcenterid, b.email, DATE(g.startdatetime) as date  " +
                                          "from game g, " +
                                          "     event e, " +
                                          "     scores s, " +
                                          "	    bowlingcenter b " +
                                          "where g.id = @gameid " +
                                          "and g.eventid = e.id " +
                                          "and e.scoresid = s.id " +
                                          "and s.bowlingcenterid = b.id ";
                    command.Parameters.AddWithValue("@gameid", Conversion.LongToSql(gameid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();
                    logger.Debug("Executed 1 " + gameid);

                    if (dataReader.Read())
                    {
                        game.bowlingcenterid = Conversion.SqlToLongOrNull(dataReader["bowlingcenterid"]).Value;
                        game.bowlingcenterName = Conversion.SqlToString(dataReader["email"]);
                        game.datePlayed = Conversion.SqlToDateTimeOrNull(dataReader["date"]).Value;
                        game.game = new SingleGame();
                    }

                    //close Data Reader
                    dataReader.Close();

                    MySqlCommand command1 = new MySqlCommand();
                    command1.Connection = databaseconnection.getConnection();
                    command1.CommandText = "select f.id, f.progressivetotal " +
                                            "from game g, " +
                                            "     frame f " +
                                            "where g.id = @gameid " +
                                            "  and f.gameid = g.id ";
                    command1.Parameters.AddWithValue("@gameid", Conversion.LongToSql(gameid));

                    List<Frame> frames = new List<Frame>();
                    MySqlDataReader dataReader1 = command1.ExecuteReader();
                    logger.Debug("Executed 2 " + gameid);    
                    //Read the data and store them in the list
                    while (dataReader1.Read())
                    {
                        Frame f = new Frame();
                        
                        f.frameid = Conversion.SqlToLongOrNull(dataReader1["id"]).Value;
                        f.progressive_total = Conversion.SqlToIntOrNull(dataReader1["progressivetotal"]).Value;
                        frames.Add(f);
                    }

                    dataReader1.Close();

                    MySqlCommand command2 = new MySqlCommand();
                    command2.Connection = databaseconnection.getConnection();
                    
                    foreach (Frame f in frames)
                    {
                        f.framescore = new Scores();

                        command2.CommandText = "SELECT total, isstrike, isspare, isgutter, issplit, isfoul, ismanuallymodified FROM bowl WHERE frameid=@frameid ORDER BY bowlNumber";
                        command2.Parameters.Clear();
                        command2.Parameters.AddWithValue("@frameid", Conversion.LongToSql(f.frameid));
    
                        MySqlDataReader dataReader2 = command2.ExecuteReader();
                        logger.Debug("Executed 3 " + gameid);

                        int bnr = 0;
                        while (dataReader2.Read())
                        {
                            if (bnr == 0)
                            {
                                f.bowl1 = new Bowl();
                                f.bowl1.pins = Conversion.SqlToIntOrNull(dataReader2["total"]).Value;
                                f.bowl1.is_strike = Conversion.SqlToBoolOrNull(dataReader2["isstrike"]).Value;
                                f.bowl1.is_spare = Conversion.SqlToBoolOrNull(dataReader2["isspare"]).Value;
                                f.bowl1.is_gutter = Conversion.SqlToBoolOrNull(dataReader2["isgutter"]).Value;
                                f.bowl1.is_split = Conversion.SqlToBoolOrNull(dataReader2["issplit"]).Value;
                                f.bowl1.is_foul = Conversion.SqlToBoolOrNull(dataReader2["isfoul"]).Value;
                                f.bowl1.is_manuallymodified = Conversion.SqlToBoolOrNull(dataReader2["ismanuallymodified"]).Value;

                                if (f.bowl1.is_strike) f.framescore.strikes++;
                                if (f.bowl1.is_spare) f.framescore.spares++;
                                if (f.bowl1.is_gutter) f.framescore.gutters++;
                                if (f.bowl1.is_split) f.framescore.splits++;
                                if (f.bowl1.is_foul) f.framescore.faults++;
                                if (f.bowl1.is_manuallymodified) f.framescore.manualmodifieds++;
                                f.framescore.total_score = f.bowl1.pins;
                            }
                            if (bnr == 1)
                            {
                                f.bowl2 = new Bowl();
                                f.bowl2.pins = Conversion.SqlToIntOrNull(dataReader2["total"]).Value - f.bowl1.pins; // tweede bowl is het aantal van bowl1 + bowl2
                                f.bowl2.is_strike = Conversion.SqlToBoolOrNull(dataReader2["isstrike"]).Value;
                                f.bowl2.is_spare = Conversion.SqlToBoolOrNull(dataReader2["isspare"]).Value;
                                f.bowl2.is_gutter = Conversion.SqlToBoolOrNull(dataReader2["isgutter"]).Value;
                                f.bowl2.is_split = Conversion.SqlToBoolOrNull(dataReader2["issplit"]).Value;
                                f.bowl2.is_foul = Conversion.SqlToBoolOrNull(dataReader2["isfoul"]).Value;
                                f.bowl2.is_manuallymodified = Conversion.SqlToBoolOrNull(dataReader2["ismanuallymodified"]).Value;

                                if (f.bowl2.is_strike) f.framescore.strikes++;
                                if (f.bowl2.is_spare) f.framescore.spares++;
                                if (f.bowl2.is_gutter) f.framescore.gutters++;
                                if (f.bowl2.is_split) f.framescore.splits++;
                                if (f.bowl2.is_foul) f.framescore.faults++;
                                if (f.bowl2.is_manuallymodified) f.framescore.manualmodifieds++;
                                f.framescore.total_score += f.bowl2.pins;
                            }
                            if (bnr == 2)
                            {
                                f.bowl3 = new Bowl();
                                f.bowl3.pins = Conversion.SqlToIntOrNull(dataReader2["total"]).Value; // derde bowl is het aantal van de derde bowl
                                f.bowl3.is_strike = Conversion.SqlToBoolOrNull(dataReader2["isstrike"]).Value;
                                f.bowl3.is_spare = Conversion.SqlToBoolOrNull(dataReader2["isspare"]).Value;
                                f.bowl3.is_gutter = Conversion.SqlToBoolOrNull(dataReader2["isgutter"]).Value;
                                f.bowl3.is_split = Conversion.SqlToBoolOrNull(dataReader2["issplit"]).Value;
                                f.bowl3.is_foul = Conversion.SqlToBoolOrNull(dataReader2["isfoul"]).Value;
                                f.bowl3.is_manuallymodified = Conversion.SqlToBoolOrNull(dataReader2["ismanuallymodified"]).Value;

                                if (f.bowl3.is_strike) f.framescore.strikes++;
                                if (f.bowl3.is_spare) f.framescore.spares++;
                                if (f.bowl3.is_gutter) f.framescore.gutters++;
                                if (f.bowl3.is_split) f.framescore.splits++;
                                if (f.bowl3.is_foul) f.framescore.faults++;
                                if (f.bowl3.is_manuallymodified) f.framescore.manualmodifieds++;
                                f.framescore.total_score += f.bowl3.pins;
                            }
                            bnr++;
                        }

                        dataReader2.Close();
                    }
                    logger.Debug("bowls doorlopen " + gameid);
                    game.game.frames = frames.ToArray();


                    //Create Command
                    MySqlCommand command3 = new MySqlCommand();
                    command3.Connection = databaseconnection.getConnection();
                    command3.CommandText = "SELECT game.total, sum(isstrike) as strikes, sum(isspare) as spares, sum(issplit) as splits, sum(isgutter) as gutters, sum(isfoul) as fouls " +
                                           "FROM game, frame, bowl " +
                                           "WHERE game.id = frame.gameid " +
                                           "AND frame.id = bowl.frameid " +
                                           "AND game.id = @gameid";
                    command3.Parameters.AddWithValue("@gameid", Conversion.LongToSql(gameid));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader3 = command3.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader3.Read())
                    {
                        game.scores = new Scores();
                        game.scores.faults = Conversion.SqlToIntOrNull(dataReader3["fouls"]).Value;
                        game.scores.gutters = Conversion.SqlToIntOrNull(dataReader3["gutters"]).Value;
                        game.scores.spares = Conversion.SqlToIntOrNull(dataReader3["spares"]).Value;
                        game.scores.splits = Conversion.SqlToIntOrNull(dataReader3["splits"]).Value;
                        game.scores.strikes = Conversion.SqlToIntOrNull(dataReader3["strikes"]).Value;
                        game.scores.total_score = Conversion.SqlToIntOrNull(dataReader3["total"]).Value;
                    }

                    //close Data Reader
                    dataReader3.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return game;
        }

        public static List<DateTime> GetDistinctGameDatesByBowlingCenter(long bowlingCenterId, long frequentbowlernumber)
        {
            List<DateTime> dates = new List<DateTime>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT DISTINCT DATE(game.startdatetime) as startdatetime FROM game, event, scores WHERE game.eventid = event.id AND event.scoresid = scores.id AND scores.bowlingcenterid=@bowlingcenterid AND game.freeentrycode=@frequentbowlernumber  GROUP BY date(game.startdatetime)";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingCenterId));
                    command.Parameters.AddWithValue("@freeentrycode", Conversion.LongToSql(frequentbowlernumber));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        dates.Add(Conversion.SqlToDateTimeOrNull(dataReader["startdatetime"]).Value);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return dates;
        }

        public static List<S_Game> GetGamesInMonthByOpenDate(DateTime bowlingDateTime)
        {
            List<S_Game> games = new List<S_Game>();
            DateTime startDate = new DateTime(bowlingDateTime.Year, bowlingDateTime.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT game.* FROM game, event, scores WHERE game.eventid = event.id AND event.scoresid = scores.id AND date(game.startdatetime)>=@startdate AND date(game.startdatetime)<@enddate";
                    command.Parameters.AddWithValue("@startdate", Conversion.DateTimeToSql(startDate));
                    command.Parameters.AddWithValue("@enddate", Conversion.DateTimeToSql(endDate));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        games.Add(DataToGameObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return games;
        }
        
        public static bool GameExistById(long id)
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
                    command.CommandText = "SELECT * FROM game WHERE id=@id";
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
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return result;
        }

        public static bool GameExistByEventId(long eventId)
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
                    command.CommandText = "SELECT * FROM game WHERE eventid=@eventid";
                    command.Parameters.AddWithValue("@eventid", Conversion.LongToSql(eventId));
                    
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
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return result;
        }

        public static bool GameExistByEventIdAndGameCode(long eventId, long gameCode)
        {
            long? gameId = null;
            return GameExistByEventIdAndGameCode(eventId, gameCode, out gameId);
        }
        
        public static bool GameExistByEventIdAndGameCode(long eventId, long gameCode, out long? gameId)
        {
            bool result = false;
            gameId = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    result = GameExistByEventIdAndGameCode(databaseconnection.getConnection(), eventId, gameCode, out gameId);

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return result;
        }

        public static bool GameExistByEventIdAndGameCode(MySqlConnection sqlConnection, long eventId, long gameCode, out long? gameId)
        {
            bool result = false;
            gameId = null;

            try
            {
                //Create Command
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;
                command.CommandText = "SELECT * FROM game WHERE eventid=@eventid AND gamecode=@gamecode";
                command.Parameters.AddWithValue("@eventid", Conversion.LongToSql(eventId));
                command.Parameters.AddWithValue("@gamecode", Conversion.LongToSql(gameCode));

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = command.ExecuteReader();

                //Read the data and store them in the list
                //Check existance
                if (dataReader.HasRows)
                {
                    result = dataReader.HasRows;
                    if (dataReader.Read())
                        gameId = DataToGameObject(dataReader).id;
                }

                //close Data Reader
                dataReader.Close();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return result;
        }

        public static int GetStrikesInGame(long gameId)
        {
            //SELECT count(1) FROM nbf.game, nbf.frame, nbf.bowl where nbf.frame.gameid = nbf.game.id and nbf.bowl.frameid = nbf.frame.id and nbf.bowl.isstrike is true;
            int strikes = 0;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT count(1) as strikes FROM game, frame, bowl WHERE game.id=@gameid and frame.gameid = game.id and bowl.frameid = frame.id and bowl.isstrike is true";
                    command.Parameters.AddWithValue("@gameid", Conversion.LongToSql(gameId));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        strikes = Conversion.SqlToIntOrNull(dataReader["strikes"]).Value;

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return strikes;
        }

        public static int GetSparesInGame(long gameId)
        {
            //SELECT count(1) FROM nbf.game, nbf.frame, nbf.bowl where nbf.frame.gameid = nbf.game.id and nbf.bowl.frameid = nbf.frame.id and nbf.bowl.isspare is true;
            int spares = 0;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT count(1) as spares FROM game, frame, bowl WHERE game.id=@gameid and frame.gameid = game.id and bowl.frameid = frame.id and bowl.isspare is true";
                    command.Parameters.AddWithValue("@gameid", Conversion.LongToSql(gameId));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        spares = Conversion.SqlToIntOrNull(dataReader["spares"]).Value;

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return spares;
        }

        public static List<S_Game> GetGamesInMonthByBowlingIdAndFreeEntryCodeAndOpenDate(long bowlingCenterId, long frequentbowlernumber, DateTime bowlingDateTime)
        {
            List<S_Game> games = new List<S_Game>();
            DateTime startDate = new DateTime(bowlingDateTime.Year, bowlingDateTime.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT game.* FROM game, event, scores WHERE game.eventid = event.id AND event.scoresid = scores.id AND scores.bowlingcenterid=@bowlingcenterid AND game.freeentrycode=@frequentbowlernumber AND date(game.startdatetime)>=@startdate AND date(game.startdatetime)<=@enddate;";
                    command.Parameters.AddWithValue("@bowlingcenterid", Conversion.LongToSql(bowlingCenterId));
                    command.Parameters.AddWithValue("@freeentrycode", Conversion.LongToSql(frequentbowlernumber));
                    command.Parameters.AddWithValue("@startdate", Conversion.DateTimeToSql(startDate));
                    command.Parameters.AddWithValue("@enddate", Conversion.DateTimeToSql(endDate));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        games.Add(DataToGameObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error reading game data: {0}", ex.Message));
            }

            return games;
        }
        
        //Insert statement
        public static long? Insert(S_Game game)
        {
            long? lastInsertedId = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    lastInsertedId = Insert(databaseconnection.getConnection(), game);

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error inserting game data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        public static long? Insert(MySqlConnection sqlConnection, S_Game game)
        {
            long? lastInsertedId = null;

            try
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "INSERT INTO game ( gamecode,  eventid,  lanenumber,  playername,  fullname,  freeentrycode,  playerposition,  startdatetime,  enddatetime,  gamenumber,  hdcp,  total) " +
                                                "VALUES (@gamecode, @eventid, @lanenumber, @playername, @fullname, @freeentrycode, @playerposition, @startdatetime, @enddatetime, @gamenumber, @hdcp, @total)";

                command.Parameters.AddWithValue("@gamecode", Conversion.LongToSql(game.gameCode));
                command.Parameters.AddWithValue("@eventid", Conversion.LongToSql(game.eventId));
                command.Parameters.AddWithValue("@lanenumber", Conversion.IntToSql(game.laneNumber));
                command.Parameters.AddWithValue("@playername", Conversion.StringToSql(game.playerName));
                if (String.IsNullOrEmpty(game.fullName))
                    command.Parameters.AddWithValue("@fullname", Conversion.StringToSql(game.playerName));
                else
                    command.Parameters.AddWithValue("@fullname", Conversion.StringToSql(game.fullName));

                command.Parameters.AddWithValue("@freeentrycode", Conversion.StringToSql(game.freeEntryCode));
                command.Parameters.AddWithValue("@playerposition", Conversion.IntToSql(game.playerPosition));
                command.Parameters.AddWithValue("@startdatetime", Conversion.DateTimeToSql(game.startDateTime));
                command.Parameters.AddWithValue("@enddatetime", Conversion.DateTimeToSql(game.endDateTime));
                command.Parameters.AddWithValue("@gamenumber", Conversion.IntToSql(game.gameNumber));
                command.Parameters.AddWithValue("@hdcp", Conversion.IntToSql(game.handicap));
                command.Parameters.AddWithValue("@total", Conversion.IntToSql(game.total));

                //Execute command
                command.ExecuteNonQuery();
                lastInsertedId = command.LastInsertedId;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error inserting game data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Game game)
        {
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //open connection
                if (databaseconnection.OpenConnection())
                {
                    Update(databaseconnection.getConnection(), game);

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error updating game data: {0}", ex.Message));
            }
        }

        public static void Update(MySqlConnection sqlConnection, S_Game game)
        {
            try
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand command = new MySqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "UPDATE game SET gamecode=@gamecode, eventid=@eventid, lanenumber=@lanenumber, playername=@playername, fullname=@fullname, freeentrycode=@freeentrycode, playerposition=@playerposition, startdatetime=@startdatetime, enddatetime=@enddatetime, gamenumber=@gamenumber, hdcp=@hdcp, total=@total WHERE id=@id ";
                if (game.fullName == null)
                    game.fullName = "";

                command.Parameters.AddWithValue("@id", Conversion.LongToSql(game.id));
                command.Parameters.AddWithValue("@gamecode", Conversion.LongToSql(game.gameCode));
                command.Parameters.AddWithValue("@eventid", Conversion.LongToSql(game.eventId));
                command.Parameters.AddWithValue("@lanenumber", Conversion.IntToSql(game.laneNumber));
                command.Parameters.AddWithValue("@playername", Conversion.StringToSql(game.playerName));
                command.Parameters.AddWithValue("@fullname", Conversion.StringToSql(game.fullName));
                command.Parameters.AddWithValue("@freeentrycode", Conversion.StringToSql(game.freeEntryCode));
                command.Parameters.AddWithValue("@playerposition", Conversion.IntToSql(game.playerPosition));
                command.Parameters.AddWithValue("@startdatetime", Conversion.DateTimeToSql(game.startDateTime));
                command.Parameters.AddWithValue("@enddatetime", Conversion.DateTimeToSql(game.endDateTime));
                command.Parameters.AddWithValue("@gamenumber", Conversion.IntToSql(game.gameNumber));
                command.Parameters.AddWithValue("@hdcp", Conversion.IntToSql(game.handicap));
                command.Parameters.AddWithValue("@total", Conversion.IntToSql(game.total));

                //Execute command
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error updating game data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM game WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error deleting game data: {0}", ex.Message));
            }
        }

        public static void Save(MySqlConnection sqlConnection, S_Event bowlEvent, S_Game game)
        {
            long? gameId = null;
            game.eventId = bowlEvent.id;

            if (!GameManager.GameExistByEventIdAndGameCode(sqlConnection, bowlEvent.id, game.gameCode, out gameId))
                game.id = Insert(sqlConnection, game).Value;
            else
            {
                game.id = gameId.Value;
                // enddatetime and total might have changed
                Update(sqlConnection, game);
            }

            foreach (S_Frame frame in game.frames)
            {
                FrameManager.Save(sqlConnection, game, frame);
            }
        }
    }
}
