using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NBF.Qubica.Common
{
    public class HMACAlgorithm
    {
        private string m_ApplicationName; //"nlfederation"; 
        private string m_SecretKey; //harderwijk = "a407ccec-b0d5-4dba-a028-8c0c62dbe5cf";

        public HMACAlgorithm(string applicationName, string secretKey)
        {
            m_ApplicationName = applicationName;
            m_SecretKey = secretKey;
        }

        public string GetAuthorizationHeader(string httpMethod, DateTime date, string url)
        {
            string messageRepresentation = GetMessageRepresentation(httpMethod, date, url); 
            string signature = CalculateSignature(m_SecretKey, messageRepresentation);
            
            return string.Format("CqApiAuth {0}:{1}", m_ApplicationName, signature);
        }

        private string GetMessageRepresentation(string httpMethod, DateTime date, string url)
        {
            //Converts date in Coordinated Universal Time (UTC) and format MM/DD/YYYY hh:mm:ss 
            string utcDate = date.ToUniversalTime().ToString(CultureInfo.InvariantCulture);

            //Extract the absolute path from url 
            string absolutePath = new Uri(url).AbsolutePath;

            return httpMethod + "\n" + utcDate + "\n" + absolutePath;
        }

        private static string CalculateSignature(string secretKey, string messageRepresentation)
        {
            HashAlgorithm hmac = GetAlgorithm(secretKey);
            byte[] messageBytes = Encoding.ASCII.GetBytes(messageRepresentation); 
            byte[] hashBytes = hmac.ComputeHash(messageBytes);
            
            return Convert.ToBase64String(hashBytes);
        }

        private static HashAlgorithm GetAlgorithm(string secretKey)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(secretKey);
            return new HMACSHA256(keyBytes);
        }
    }
}
