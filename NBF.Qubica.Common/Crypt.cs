using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NBF.Qubica.Common
{
    public static class Crypt
    {
        static Logger logger = LogManager.GetCurrentClassLogger();
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("Krjdaher");

        public static string Encrypt(string originalString)
        {
            string result = null;

            if (!String.IsNullOrEmpty(originalString))
            {
                try
                {
                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(originalString);
                                streamWriter.Flush();
                                cryptoStream.FlushFinalBlock();
                                streamWriter.Flush();

                                result = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format("Error encrypting: '{0}': {1}", originalString, ex.Message));
                }
            }
            else
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");

            return result;
        }

        public static string Decrypt(string cryptedString)
        {
            string result = "";

            if (!String.IsNullOrEmpty(cryptedString))
            {
                try
                {
                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString)))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format("Error decrypting '{0}': {1}", cryptedString, ex.Message));
                }
            }
            else
            {
                //throw new ArgumentNullException
                //   ("The string which needs to be decrypted can not be null.");
            }

            return result;
        }

    }
}
