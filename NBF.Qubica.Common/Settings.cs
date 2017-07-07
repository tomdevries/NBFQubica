using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Web;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace NBF.Qubica.Common
{
    public static class Settings
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static MethodBase MethodName()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1); // name of the calling methode

            return sf.GetMethod();
        }

        #region Database Settings
        public static string Server
        {
            get
            {
                return GetSetting("SERVER");
            }
        }

        public static string Database
        {
            get
            {
                return GetSetting("DATABASE");
            }
        }

        public static string Uid
        {
            get
            {
                return GetSetting("UID");
            }
        }

        public static string Password
        {
            get
            {
                return GetSetting("PASSWORD");
            }
        }
        #endregion

        #region Polling Service Settings
        public static double Interval
        {
            get
            {
                try
                {

                    return Convert.ToDouble(GetSetting("INTERVAL"), CultureInfo.InvariantCulture) * 1000;
                }
                catch
                {
                }

                return 1000;
            }
        }
        #endregion

        #region Managementportal
        public static int Sessiontimeout
        {
            get
            {
                try
                {
                    return int.Parse(GetSetting("SESSIONTIMEOUT"));
                }
                catch
                {
                }
                return 1;
            }
        }
        #endregion

        #region Mail
        public static string MailServer
        {
            get
            {
                return GetSetting("MAILSERVER");
            }
        }

        public static string MailUser
        {
            get
            {
                return GetSetting("MAILUSER");
            }
        }

        public static string MailPassword
        {
            get
            {
                return GetSetting("MAILPASSWORD");
            }
        }
        #endregion

        #region WebSite
        public static string Url
        {
            get
            {
                return GetSetting("URL");
            }
        }        
        #endregion

        private static string GetSetting(string key)
        {
            string returnValue = null;

            try
            {
                returnValue = ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetSetting() Onbekende fout bij het ophalen van setting '{0}' : {1}", key, ex.Message));
                throw;
            }

            return returnValue;
        }
    }
}
