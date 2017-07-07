using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NBF.Qubica.Common;
using NLog;

namespace NBF.Qubica.Database
{
    public class DatabaseConnection
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private MySqlConnection connection;

        //Constructor
        public DatabaseConnection()
        {
            connection = new MySqlConnection(String.Concat("SERVER=", Settings.Server, ";", "DATABASE=", Settings.Database, ";", "UID=", Settings.Uid, ";", "PASSWORD=", Common.Crypt.Decrypt(Settings.Password), ";"));
        }

        //Open connection to database
        public bool OpenConnection()
        {
            try
            {
                logger.Debug("Try to open....");
                connection.Open();
                logger.Debug("Database opened");
                return true;
            }
            catch (MySqlException ex)
            {
                logger.Error(ex.Message);
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        throw new Exception("Cannot connect to server.  Contact administrator");
                    case 1042:
                        throw new Exception("Can't get hostname address");
                    case 1045:
                        throw new Exception("Invalid username/password, please try again");
                }
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                logger.Debug("Database closed");

                return true;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public MySqlConnection getConnection()
        {
            return connection;
        }
    }
}
