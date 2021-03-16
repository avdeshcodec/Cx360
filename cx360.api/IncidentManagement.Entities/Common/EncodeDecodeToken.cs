using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Common
{
   
       public class EncodeDecodeToken
        {
           
            public static string DecrypteAuthenticateTicket(string EncryptToken)
            {

                // here we get planEmail : encrypted password
                var decryptedToken = AesEncryptionDecryption.Decrypt(EncryptToken);

                return decryptedToken;

            }

            public static string Base64Encode(string plainText)
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }

            public static string Base64Decode(string base64EncodedData)
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
        }
}
