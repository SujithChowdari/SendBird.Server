using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SendBird.Api.Utilities
{
    public static class Helper
    {
        public static string GetHMAC(string text, string key)
        {
            key = key ?? "";

            string hash;
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] code = encoder.GetBytes(key);
            using (HMACSHA256 hmac = new HMACSHA256(code))
            {
                Byte[] hmBytes = hmac.ComputeHash(encoder.GetBytes(text));
                hash = ToHexString(hmBytes);
            }
            return hash;
        }

        public static string ToHexString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public static string ReadRequestBodyIntoString(HttpContext httpContext)
        {
            try
            {
                var bodyString = "";
                var req = httpContext.Request;

                // Allows using several time the stream in ASP.Net Core
                req.EnableBuffering();

                // Arguments: Stream, Encoding, detect encoding, buffer size 
                // AND, the most important: keep stream opened
                using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyString = reader.ReadToEnd();
                }

                // Rewind, so the core is not lost when it looks the body for the request
                req.Body.Position = 0;

                return bodyString;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
