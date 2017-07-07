using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using NLog;
using NBF.Qubica.Common;

namespace NBF.Qubica.RestClient
{
    public static class RestfullClient
    {
        private const string APPLICATION_NAME = "Conqueror";
        private const string SECRET_KEY = "C7DD662A-8AAD-4EA0-BE7F-5CE43586E950";

        static Logger logger = LogManager.GetCurrentClassLogger();

        public static string GetAPIVersion(string uri, int? centerId, string appname, string secretkey)
        {
            #region Build the uri
            uri = string.Concat(uri + "rest/" + centerId + "/ScoreInfo/Version");
            #endregion

            return GetResponse(uri, "version.xml", appname, secretkey);
        }

        public static string GetScores(string uri, int? centerId, string lanes, DateTime? fromDate, DateTime? toDate, string appname, string secretkey)
        {
            #region Build the uri
            uri = string.Concat(uri + "rest/" + centerId + "/ScoreInfo/Scores");

            if (!String.IsNullOrEmpty(lanes))
                uri = string.Concat(uri, "?lanes=", lanes);

            if (!String.IsNullOrEmpty(lanes) && fromDate != null)
                uri = string.Concat(uri, "&fromDate=", fromDate.Value.ToString("yyyyMMdd"));

            if (String.IsNullOrEmpty(lanes) && fromDate != null)
                uri = string.Concat(uri, "?fromDate=", fromDate.Value.ToString("yyyyMMdd"));

            if ((fromDate != null && toDate != null) || (!String.IsNullOrEmpty(lanes) && fromDate == null && toDate != null))
                uri = string.Concat(uri, "&toDate=", toDate.Value.ToString("yyyyMMdd"));

            if (String.IsNullOrEmpty(lanes) && fromDate == null && toDate != null)
                uri = string.Concat(uri, "?toDate=", toDate.Value.ToString("yyyyMMdd"));

            logger.Info(String.Format("uri: {0}", uri));
            #endregion

            return GetResponse(uri, "scores.xml", appname, secretkey);
        }

        public static string GetLastScores(string uri, int? centerId, string lanes, string appname, string secretkey)
        {
            #region Build the uri
            uri = string.Concat(uri + "rest/" + centerId + "/ScoreInfo/Scores");

            if (!String.IsNullOrEmpty(lanes))
                uri = string.Concat(uri, "/Last/", lanes);

            logger.Info(String.Format("uri: {0}", uri));
            #endregion

            return GetResponse(uri, "scores.xml", appname, secretkey);
        }

        #region Helper functions
        public static string GetResponse(string uri, string debugFileContent, string appname, string secretkey)
        {
            try
            {
                var dt = DateTime.Now;
                HMACAlgorithm hmac = new HMACAlgorithm(appname, secretkey);
                var auth = hmac.GetAuthorizationHeader("GET", dt, uri);
                var request = (HttpWebRequest)WebRequest.Create(uri);

                request.Method = "GET";
                request.Date = dt.ToUniversalTime();
                request.KeepAlive = false;
                request.Headers.Add(HttpRequestHeader.Authorization, auth);

#if DEBUG
                return new StreamReader(debugFileContent).ReadToEnd();
#else
                using (var response = request.GetResponse())
                {
                    return ReadResponse(response);
                }
#endif
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Error requesting (\"{0}\"): {1}", uri, ex.Message));
                throw ex;
            }
        }

        public static string ReadResponse(WebResponse response)
        {
            if (response == null || response.ContentLength <= 0)
                return "";

            using (var reader = new BinaryReader(response.GetResponseStream()))
            {
                var data = reader.ReadBytes((int)response.ContentLength);
                return Encoding.UTF8.GetString(data);
            }
        }

        private static string GetSignature(string key, byte[] messageToSendBytes)
        {
            var keyHMAC = new HMACSHA1(Encoding.ASCII.GetBytes(key));
            var keyBytes = keyHMAC.ComputeHash(messageToSendBytes);
            return Convert.ToBase64String(keyBytes);
        }

        private static string GetAuthorizationHeader(string uri)
        {
            string applicationName = "Conqueror";
            string secretKey = "C7DD662A-8AAD-4EA0-BE7F-5CE43586E950";

            var messageToSendBytes = Encoding.ASCII.GetBytes(uri);

            var secretKeySignature = GetSignature(secretKey, messageToSendBytes);
            return string.Format("{0}:{1}", applicationName, secretKeySignature);
        }
        #endregion
    }
}
