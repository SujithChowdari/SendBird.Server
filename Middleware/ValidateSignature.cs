using Microsoft.AspNetCore.Http;
using SendBird.Api.Models;
using SendBird.Api.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendBird.Api.Middleware
{
    public class ValidateSignature
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationConfig _appConfig;

        public ValidateSignature(RequestDelegate next, ApplicationConfig appConfig)
        {
            _next = next;
            _appConfig = appConfig;
        }

        public async Task Invoke(HttpContext context)
        {            
            string requestBody = Helper.ReadRequestBodyIntoString(context);
            //requestBody = System.Text.RegularExpressions.Regex.Unescape(requestBody);
            string requestSignature = context.Request.Headers["X-SendBird-Signature"];

            //do the checking
            if (requestSignature == null)
            {
                context.Response.StatusCode = 401;
                throw new Exception("Unauthorized call from webhook to the api");
            }
            else
            {
                var calculatedSignature = Helper.GetHMAC(requestBody, _appConfig.MasterApiToken);
                if(requestSignature == calculatedSignature)
                {
                    //pass request further if correct
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    throw new Exception($"Invalid signature. calculated signature {calculatedSignature}");
                }
            }           
        }      
    }    
}
