using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Common
{
   
       public class EncodeDecodeToken
        {
            //public static string CreateEncryptedStaffAuthenticateTicket(Entities.Request.TokenRequest tokenRequest)
            //{
            //    var userNameAndSessionId = tokenRequest.Key + ':' + tokenRequest.Secret;
            //    var token = AesEncryptionDecryption.Encrypt(userNameAndSessionId);
            //    return token;
            //}

            //public static string CreateEncryptedUserAuthenticatePasswordTicket(Entities.UserRequest.UserLoginRequest userLoginRequest, string Key, string Secret)
            //{
            //    var planEmailEncryptedPassord = Key + ':' + Secret + ':' + userLoginRequest.UserName + ':' + userLoginRequest.APILogId + ':' + userLoginRequest.CompanyId;
            //    var token = AesEncryptionDecryption.Encrypt(planEmailEncryptedPassord);
            //    return token;
            //}

            //public static string CreateEncryptedStaffLoginTicket(Entities.Request.StaffTokenRequest staffTokenRequest)
            //{
            //    var staffLoginToken = staffTokenRequest.Key + ':' + staffTokenRequest.Secret + ':' + staffTokenRequest.UserName + ':' + staffTokenRequest.ClientId + ':' + staffTokenRequest.CompanyId;
            //    string token = AesEncryptionDecryption.Encrypt(staffLoginToken);
            //    return token;
            //}

            //public static string CreateEncryptedHubAuthenticatePasswordTicket(Entities.Request.HubTokenRequest hubTokenRequest)
            //{
            //    var planEmailEncryptedPassord = hubTokenRequest.HubKey + ':' + hubTokenRequest.HubSecret + ':' + hubTokenRequest.APILogId;
            //    var token = AesEncryptionDecryption.Encrypt(planEmailEncryptedPassord);
            //    return token;
            //}
            //public static string CreateEncryptedCourtLinkAuthenticatePasswordTicket(Entities.Request.CourtLinkUserLoginRequest courtLinkUserLoginRequest)
            //{
            //    var planEmailEncryptedPassord = courtLinkUserLoginRequest.UserName + ':' + courtLinkUserLoginRequest.Password + ':' + courtLinkUserLoginRequest.APILogId;
            //    var token = AesEncryptionDecryption.Encrypt(planEmailEncryptedPassord);
            //    return token;
            //}

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
