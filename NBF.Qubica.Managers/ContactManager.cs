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
    public static class ContactManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        private static S_Contact DataToObject(MySqlDataReader dataReader)
        {
            S_Contact contact = new S_Contact();

            contact.id = Conversion.SqlToLongOrNull(dataReader["id"]).Value;
            contact.email = Conversion.SqlToString(dataReader["email"]);
            contact.message = Conversion.SqlToString(dataReader["message"]);

            return contact;
        }

        //Insert statement
        public static long? Insert(S_Contact contact)
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
                    command.CommandText = "INSERT INTO contact (email, message) VALUES (@email, @message)";
                    command.Parameters.AddWithValue("@email", Conversion.StringToSql(contact.email));
                    command.Parameters.AddWithValue("@message", Conversion.StringToSql(contact.message));

                    //Execute command
                    command.ExecuteNonQuery();
                    lastInsertedId = command.LastInsertedId;

                    //close connection
                    databaseconnection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Insert, Error inserting contact data: {0}", ex.Message));
                throw;
            }

            return lastInsertedId;
        }
    }
}