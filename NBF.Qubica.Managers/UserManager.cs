using System;
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
    public static class UserManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_User DataToObject(MySqlDataReader dataReader)
        {
            S_User user = new S_User();

            user.id = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
            user.name = Conversion.SqlToString(dataReader["name"]);
            user.address = Conversion.SqlToString(dataReader["address"]);
            user.email = Conversion.SqlToString(dataReader["email"]);
            user.logindatetime = Conversion.SqlToLongOrNull(dataReader["logindatetime"]);
            user.password = Conversion.SqlToString(dataReader["password"]);
            user.roleid = (Role)Conversion.SqlToIntOrNull(dataReader["roleid"]);
            user.username = Conversion.SqlToString(dataReader["username"]);
            user.city = Conversion.SqlToString(dataReader["city"]);
            user.isMember = Conversion.SqlToBool(dataReader["ismember"]);
            user.memberNumber = Conversion.SqlToIntOrNull(dataReader["membernumber"]);
            user.isRegistrationConfirmed = Conversion.SqlToBool(dataReader["isregistrationconfirmed"]);
            if (!string.IsNullOrEmpty(Conversion.SqlToString(dataReader["frequentbowlernumber"])))
                user.frequentbowlernumber = Conversion.SqlToLongOrNull(dataReader["frequentbowlernumber"]).Value;

            return user;
        }

        public static List<S_User> GetUsers()
        {
            List<S_User> users = new List<S_User>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM user";

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        users.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUsers, Error reading users data: {0}", ex.Message));
            }

            return users;
        }

        public static List<S_User> GetUsersByName(string name)
        {
            List<S_User> users = new List<S_User>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM user WHERE name LIKE '%" + name + "%'";

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        users.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUsersByName, Error reading users data: {0}", ex.Message));
            }

            return users;
        }

        public static bool IsLoggedIn(long id)
        {
            bool result = false;
            
            TimeSpan? ts = UserLoggedInTimespan(id);
            if (ts != null)
            {
                if (ts.Value.TotalMinutes < 1)
                    result = true;

                S_User user = UserManager.GetUserById(id);
                if (user != null)
                {
                    if (result)
                        user.logindatetime = DateTime.Now.Ticks;
                    else
                        user.logindatetime = null;
                    Update(user);
                }
            }
            
            return result;
        }

        private static TimeSpan? UserLoggedInTimespan(long id)
        {
            TimeSpan? result = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    S_User user = null;

                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM user WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data to check if found
                    if (dataReader.Read())
                    {
                        user = DataToObject(dataReader);
                        if (user.logindatetime != null)
                        {
                            long elapsedTicks = DateTime.Now.Ticks - (long)user.logindatetime;
                            result  = new TimeSpan(elapsedTicks);
                        }
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("IsLoggedIn, Error reading user data: {0}", ex.Message));
            }

            return result;
        }

        public static S_User GetUserByName(string name)
        {
            S_User user = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM user WHERE name=@name";
                    command.Parameters.AddWithValue("@name", Conversion.StringToSql(name));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        user = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUserByName, Error reading user data: {0}", ex.Message));
            }

            return user;
        }

        public static S_User GetUserByEmail(string email)
        {
            S_User user = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM user WHERE email=@email";
                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(email));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        user = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUserByEmail, Error reading user data: {0}", ex.Message));
            }

            return user;
        }

        public static S_User GetUserById(long id)
        {
            S_User user = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM user WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        user = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUserById, Error reading user data: {0}", ex.Message));
            }

            return user;
        }

        public static S_User GetUserByNameAndPassword(string username, string password)
        {
            S_User user = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM user WHERE username=@username AND password=@password";
                    command.Parameters.AddWithValue("@username", Conversion.StringToSql(username));
                    command.Parameters.AddWithValue("@password", Conversion.StringToSql(password));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        user = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUserByNameAndPassword, Error reading user data: {0}", ex.Message));
            }

            return user;
        }

        public static S_User GetUserByNamePasswordAndFrequentbowlernumber(string username, string password, long frequentbowlernumber)
        {
            S_User user = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM user WHERE username=@username AND password=@password AND frequentbowlernumber=@frequentbowlernumber";
                    command.Parameters.AddWithValue("@username", Conversion.StringToSql(username));
                    command.Parameters.AddWithValue("@password", Conversion.StringToSql(password));
                    command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(frequentbowlernumber));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        user = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUserByNamePasswordAndFrequentbowlernumber, Error reading user data: {0}", ex.Message));
            }

            return user;
        }

        public static S_User GetUserByFrequentbowlernumber(long frequentbowlernumber)
        {
            S_User user = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM user WHERE frequentbowlernumber=@frequentbowlernumber";
                    command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(frequentbowlernumber));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    if (dataReader.Read())
                        user = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUserByFrequentbowlernumber, Error reading user data: {0}", ex.Message));
            }

            return user;
        }

        public static bool UserExistById(long id)
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
                    command.CommandText = "SELECT * FROM user WHERE id=@id";
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
                logger.Error(string.Format("UserExistById, Error reading user data: {0}", ex.Message));
            }

            return result;
        }

        public static bool UserExistByEmail(string email)
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
                    command.CommandText = "SELECT * FROM user WHERE email=@email";
                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(email));

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
                logger.Error(string.Format("UserExistByEmail, Error reading user data: {0}", ex.Message));
            }

            return result;
        }

        public static bool UserExistByFrequentBowlerNumber(long frequentbowlernumber)
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
                    command.CommandText = "SELECT * FROM user WHERE frequentbowlernumber=@frequentbowlernumber";
                    command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(frequentbowlernumber));

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
                logger.Error(string.Format("UserExistByEmail, Error reading user data: {0}", ex.Message));
            }

            return result;
        }

        public static bool UserExistByUsername(string username)
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
                    command.CommandText = "SELECT * FROM user WHERE username=@username";
                    command.Parameters.AddWithValue("@username", Conversion.StringToSql(username));

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
                logger.Error(string.Format("UserExistByUsername, Error reading user data: {0}", ex.Message));
            }

            return result;
        }

        public static bool UsernameHasGames(string username, long frequentbowlernumber)
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
                    command.CommandText = "SELECT * FROM game WHERE UPPER(playername)=UPPER(@username) AND freeentrycode = @frequentbowlernumber";
                    command.Parameters.AddWithValue("@username", Conversion.StringToSql(username));
                    command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(frequentbowlernumber));

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
                logger.Error(string.Format("UsernameHasGames, Error reading user data: {0}", ex.Message));
            }

            return result;
        }

        //Insert statement
        public static long? Insert(S_User user)
        {
            long? lastInsertedId = null;
            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();


                if (!UserExistByEmail(user.email))
                {
                    if (!UserExistByFrequentBowlerNumber(user.frequentbowlernumber))
                    {
                        //open connection
                        if (databaseconnection.OpenConnection())
                        {
                            //create command and assign the query and connection from the constructor
                            MySqlCommand command = new MySqlCommand();
                            command.Connection = databaseconnection.getConnection();
                            command.CommandText = "INSERT INTO user (name, password, roleid, logindatetime, email, address, username, city, ismember, membernumber, isregistrationconfirmed, frequentbowlernumber) VALUES (@name, @password, @roleid, @logindatetime, @email, @address, @username, @city, @ismember, @membernumber, @isregistrationconfirmed, @frequentbowlernumber)";
                            command.Parameters.AddWithValue("@name", Conversion.StringToSql(user.name));
                            command.Parameters.AddWithValue("@password", Conversion.StringToSql(user.password));
                            command.Parameters.AddWithValue("@roleid", Conversion.IntToSql((int)user.roleid));
                            command.Parameters.AddWithValue("@logindatetime", Conversion.DoubleToSql(user.logindatetime));
                            command.Parameters.AddWithValue("@email", Conversion.StringToSql(user.email));
                            command.Parameters.AddWithValue("@address", Conversion.StringToSql(user.address));
                            command.Parameters.AddWithValue("@username", Conversion.StringToSql(user.username));
                            command.Parameters.AddWithValue("@city", Conversion.StringToSql(user.city));
                            command.Parameters.AddWithValue("@ismember", Conversion.BoolToSql(user.isMember));
                            command.Parameters.AddWithValue("@membernumber", Conversion.IntToSql(user.memberNumber));
                            command.Parameters.AddWithValue("@isregistrationconfirmed", Conversion.BoolToSql(user.isRegistrationConfirmed));
                            command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(user.frequentbowlernumber));

                            //Execute command
                            command.ExecuteNonQuery();
                            lastInsertedId = command.LastInsertedId;

                            //close connection
                            databaseconnection.CloseConnection();
                        }
                    }
                    else
                    {
                        throw (new Exception("Frequent bowler nummer is reeds in gebruik"));
                    }
                }
                else
                {
                    throw (new Exception("Email adres is reeds in gebruik"));
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting uUser data: {0}", ex.Message));
                throw;
            }

            return lastInsertedId;
        }

        //Update statement
        public static void Update(S_User user)
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

                    command.CommandText = "UPDATE user SET name=@name, password=@password, roleid=@roleid, logindatetime=@logindatetime, email=@email, address=@address, username=@username, city=@city, ismember=@ismember, membernumber=@membernumber, isregistrationconfirmed=@isregistrationconfirmed, frequentbowlernumber=@frequentbowlernumber WHERE id=@id ";

                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(user.id));
                    command.Parameters.AddWithValue("@name", Conversion.StringToSql(user.name));
                    command.Parameters.AddWithValue("@password", Conversion.StringToSql(user.password));
                    command.Parameters.AddWithValue("@roleid", Conversion.IntToSql((int)user.roleid));
                    command.Parameters.AddWithValue("@logindatetime", Conversion.DoubleToSql(user.logindatetime));
                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(user.email));
                    command.Parameters.AddWithValue("@address", Conversion.StringToSql(user.address));
                    command.Parameters.AddWithValue("@username", Conversion.StringToSql(user.username));
                    command.Parameters.AddWithValue("@city", Conversion.StringToSql(user.city));
                    command.Parameters.AddWithValue("@ismember", Conversion.BoolToSql(user.isMember));
                    command.Parameters.AddWithValue("@membernumber", Conversion.IntToSql(user.memberNumber));
                    command.Parameters.AddWithValue("@isregistrationconfirmed", Conversion.BoolToSql(user.isRegistrationConfirmed));
                    command.Parameters.AddWithValue("@frequentbowlernumber", Conversion.LongToSql(user.frequentbowlernumber));

                    //Execute command
                    command.ExecuteNonQuery();

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating user data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM user WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting user data: {0}", ex.Message));
            }
        }
    }
}
