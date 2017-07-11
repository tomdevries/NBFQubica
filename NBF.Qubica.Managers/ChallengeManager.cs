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
    public class ChallengeManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Challenge DataToChallengeObject(MySqlDataReader dataReader)
        {
            S_Challenge challenge = new S_Challenge();

            challenge.id = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
            challenge.name = Conversion.SqlToString(dataReader["name"]);

            return challenge;
        }

        public static S_Challenge GetChallenge(long id)
        {
            S_Challenge challenge = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM challenge WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        challenge = DataToChallengeObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetChallenge, Error reading challenge data: {0}", ex.Message));
            }

            return challenge;
        }

        public static S_Challenge GetChallengeByCompetition(long id)
        {
            S_Challenge challenge = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT ch.* FROM challenge ch, competition co WHERE ch.id = co.challengeid AND co.id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        challenge = DataToChallengeObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetChallenge, Error reading challenge data: {0}", ex.Message));
            }

            return challenge;
        }
        public static List<S_Challenge> GetChallenges()
        {
            List<S_Challenge> challenges = new List<S_Challenge>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM challenge";

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        challenges.Add(DataToChallengeObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetChallenges, Error reading challenge data: {0}", ex.Message));
            }

            return challenges;
        }

    }
}
