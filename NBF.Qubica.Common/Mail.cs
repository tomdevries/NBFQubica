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

        public static void SendRegistrationMessage(string to,  string name, string idnaam, long idnummer)
        {
            // Create a message and set up the recipients.
            MailMessage message = new MailMessage("noreply@bowlingscores.nl", to);
            message.Subject = "Registatie NBF Score website";
            
            StringBuilder body = new StringBuilder();
            body.AppendLine("Beste " + name + ",");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Je hebt jezelf aangemeld met de volgende gegevens:");
            body.AppendLine("ID-Naam: " + idnaam);
            body.AppendLine("ID-nummer: " + idnummer);
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Klik op onderstaande link om de registratie te voltooien:");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine(Settings.Url + "/Confirm.aspx?idnummer=" + idnummer);
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
                logger.Error("Exception caught in SendRegistrationMessage({0},{1}): {2}", to, name, ex.ToString());
            }
        }

        public static void SendActivationMessage(string to, string name, string idnaam, long idnummer)
        {
            // Create a message and set up the recipients.
            MailMessage message = new MailMessage("noreply@bowlingscores.nl", to);
            message.Subject = "Activatie NBF Score website account";

            StringBuilder body = new StringBuilder();
            body.AppendLine("Beste " + name + ",");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Je hebt je zelf aangemeld met de volgende gegevens:");
            body.AppendLine("ID-Naam: " + idnaam);
            body.AppendLine("ID-nummer: " + idnummer);
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Je account is geactiveerd.");
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
                logger.Error("Exception caught in SendActivationMessage({0},{1}): {2}", to, name, ex.ToString());
            }
        }

        public static void SendForgottenMessage(string to, string name, string idnaam, long idnummer)
        {
            logger.Debug("We gaan de mail opbouwen...");

            // Create a message and set up the recipients.
            MailMessage message = new MailMessage("noreply@bowlingscores.nl", to);
            message.Subject = "Account informatie NBF Score website";

            StringBuilder body = new StringBuilder();
            body.AppendLine("Beste " + name + ",");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Je hebt aangegeven je account gegeven te willen ontvangen:");
            body.AppendLine("ID-Naam: " + idnaam);
            body.AppendLine("ID-nummer: " + idnummer);
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Klik op onderstaande link om je wachtwoord opnieuw in te stellen:");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine(Settings.Url + "/Reset.aspx?idnummer=" + idnummer);
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Met vriendelijke groet,");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Nederlandse Bowling Federatie");

            message.Body = body.ToString();

            //Send the message.
            logger.Debug("Mail client maken...");
            SmtpClient client = new SmtpClient();
            client.Host = Settings.MailServer;
            client.Credentials = new System.Net.NetworkCredential(Settings.MailUser, Crypt.Decrypt(Settings.MailPassword));
            logger.Debug("Mail client gemaakt.");

            try
            {
                logger.Debug("We gaan een mail sturen...");
                client.Send(message);
                logger.Debug("Mail gestuurd.");
            }
            catch (Exception ex)
            {
                logger.Error("Exception caught in SendForgottenMessage({0},{1}): {2}", to, name, ex.ToString());
            }
        }

        public static void SendPasswordUpdatedMessage(string to, string name, string idnaam, long idnummer)
        {
            // Create a message and set up the recipients.
            MailMessage message = new MailMessage("noreply@bowlingscores.nl", to);
            message.Subject = "Account informatie NBF Score website";

            StringBuilder body = new StringBuilder();
            body.AppendLine("Beste " + name + ",");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("Je wachtwoord is succesvol aangepast.");
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
                logger.Error("Exception caught in SendPasswordUpdatedMessage({0},{1}): {2}", to, name, ex.ToString());
            }
        }

        public static void SendContactMailToNBF(string to, string name, string mailadres, string question)
        {
            // Create a message and set up the recipients.
            MailMessage message = new MailMessage("noreply@bowlingscores.nl", to);
            message.Subject = "Contact/vraag via NBF Score website";

            StringBuilder body = new StringBuilder();
            body.AppendLine("De gebruiker " + name + ",  met e-mail adres " + mailadres + " , heeft de volgende vraag gesteld:");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine(question);
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
                logger.Error("Exception caught in SendContactMailToNBF({0},{1}): {2}", to, mailadres, ex.ToString());
            }
        }
    }
}
