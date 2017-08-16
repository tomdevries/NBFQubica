using MySql.Data.MySqlClient;
using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Database;
using NLog;
using System;
using System.Collections.Generic;

namespace NBF.Qubica.Managers
{
    public static class TextManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Text DataToObject(MySqlDataReader dataReader)
        {
            S_Text text = new S_Text();

            text.id = Conversion.SqlToIntOrNull(dataReader["id"]).Value;
            text.label = Conversion.SqlToString(dataReader["label"]);
            text.text = Conversion.SqlToString(dataReader["text"]);

            return text;
        }

        public static List<S_Text> GetTexts()
        {
            List<S_Text> texts = new List<S_Text>();

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM text";

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                        texts.Add(DataToObject(dataReader));

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetTexts, Error reading texts data: {0}", ex.Message));
            }

            return texts;
        }

        public static S_Text GetTextById(long id)
        {
            S_Text text = null;

            try
            {
                DatabaseConnection databaseconnection = new DatabaseConnection();

                //Open connection
                if (databaseconnection.OpenConnection())
                {
                    //Create Command
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = databaseconnection.getConnection();
                    command.CommandText = "SELECT * FROM text WHERE id=@id";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = command.ExecuteReader();

                    //Read the data and store them in the object
                    if (dataReader.Read())
                        text = DataToObject(dataReader);

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetTextsById, Error reading text data: {0}", ex.Message));
            }

            return text;
        }
        //Insert statement
        public static long? Insert(S_Text text)
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

                    command.CommandText = "INSERT INTO text ( label,  text) VALUES (@label, @text)";

                    command.Parameters.AddWithValue("@label", Conversion.StringToSql(text.label));
                    command.Parameters.AddWithValue("@text", Conversion.StringToSql(text.text));


                    //Execute command
                    command.ExecuteNonQuery();
                    lastInsertedId = command.LastInsertedId;

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting text data: {0}", ex.Message));
            }

            return lastInsertedId.Value;
        }

        //Update statement
        public static void Update(S_Text text)
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

                    command.CommandText = "UPDATE text SET label=@label, text=@text WHERE id=@id ";

                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(text.id));
                    command.Parameters.AddWithValue("@label", Conversion.StringToSql(text.label));
                    command.Parameters.AddWithValue("@text", Conversion.StringToSql(text.text));

                    //Execute command
                    command.ExecuteNonQuery();

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Update, Error updating text data: {0}", ex.Message));
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
                    command.CommandText = "DELETE FROM text WHERE id=@id ";
                    command.Parameters.AddWithValue("@id", Conversion.LongToSql(id));

                    command.ExecuteNonQuery();

                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Delete, Error deleting text data: {0}", ex.Message));
                throw new Exception("Er is een fout opgetreden bij het verwijderen");
            }
        }
    }
}
