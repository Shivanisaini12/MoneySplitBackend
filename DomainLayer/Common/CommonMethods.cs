using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Common
{
    public static class CommonMethods
    {
        private static string key = "jsldfsd76fnxv6sdtfwefhneuy43t7rfb34ygr435ghgtn45hg45ntb4fcy4r5hj45otyrnvbr4h45v";
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string Encryptword(string Encryptval)
        {
            byte[] SrctArray;
            byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(Encryptval);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();
            SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objcrpt.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateEncryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);
            objt.Clear();
            return Convert.ToBase64String(resArray, 0, resArray.Length);
        }
        public static string Decryptword(string DecryptText)
        {
            byte[] SrctArray;
            byte[] DrctArray = Convert.FromBase64String(DecryptText);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objmdcript = new MD5CryptoServiceProvider();
            SrctArray = objmdcript.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objmdcript.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateDecryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(DrctArray, 0, DrctArray.Length);
            objt.Clear();
            return UTF8Encoding.UTF8.GetString(resArray);
        }

        //public static bool CheckClaimsIdentity(IEnumerable<Claim> userClaims)
        //{
        //    try
        //    {
        //        if(userClaims != null )
        //        {
        //            if(userClaims.Count > 0)
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (FormatException)
        //    {
        //        return false;
        //    }
        //}

        public static DateTime GetFirstDayOfWeek(DateTime date)
        {
            DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int offset = fdow - date.DayOfWeek;
            DateTime fdowDate = date.AddDays(offset);
            return fdowDate;
        }
        public static DateTime GetLastDayOfWeek(DateTime date)
        {
            DateTime ldowDate = GetFirstDayOfWeek(date).AddDays(6);
            return ldowDate;
        }
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }
        public static DateTime GetLastDayOfYear(DateTime date)
        {
            DateTime newdate = new DateTime(date.Year + 1, 1, 1);
            return newdate.AddDays(-1);
        }
        public static DateTime GetLastDayOfQuarterMonth(DateTime date)
        {
            var result = date.Date.AddDays(1 - date.Day).AddMonths(3 - (date.Month - 1) % 3).AddDays(-1);
            return result;
        }

        public static string GetTokenFromResponseData(string responseData)
        {
            try
            {

                if (string.IsNullOrEmpty(responseData.Trim()))
                {
                    var a = JsonConvert.DeserializeObject<TokenClass>(responseData);
                    if(a != null && String.IsNullOrEmpty(a.token))
                    {
                        return a.token;
                    }
                    return "";
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            IEnumerable<Claim> claims = securityToken.Claims;
            return claims;
        }
        class TokenClass
        {
            public string token { get; set; }
        }
    }
}
