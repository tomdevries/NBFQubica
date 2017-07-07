using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace NBF.Qubica.Common
{
    public static class Mail
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static void SendMessage(string to,  string name, string code)
        {
            // Create a message and set up the recipients.
            MailMessage message = new MailMessage("noreply@nbf.nl", to);
            message.Subject = "Registatie NBF Score website";
            
            StringBuilder body = new StringBuilder();
            body.AppendLine("Beste " + name + ",");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Klik op onderstaande link om de registratie te voltooien:");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine(Settings.Url + "Account/Confirm/" + code);
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Met vriendelijke groet,");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Nederlandse Bowling Federatie");

            message.Body = body.ToString();

            //Send the message.
            SmtpClient client = new SmtpClient();
            client.Host = Settings.MailServer;
            client.Credentials = new System.Net.NetworkCredential(Settings.MailUser, Crypt.Decrypt(Settings.MailPassword));

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                logger.Error("Exception caught in SendMessage({0},{1}): {2}", to, name, ex.ToString());
            }
        }
    }
}
