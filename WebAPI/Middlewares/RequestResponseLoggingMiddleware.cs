using DomainLayer.Common;
using DomainLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Service_Layer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        static dynamic responseData = "";
        static dynamic requestData = "";
        static dynamic query = "";

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //First, get the incoming request
            var request = await FormatRequest(context.Request);
            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using var responseBody = new MemoryStream();
            //...and use that for the temporary response body
            context.Response.Body = responseBody;

            //Continue down the Middleware pipeline, eventually returning to this class
            await _next(context);

            //Format the response from the server
            var response = await FormatResponse(context.Response);

            using (var _context = new RxSplitterContext())
            {
                var a = context.Request.HttpContext.User.Identity as ClaimsIdentity;
                var b = "";
                if (a?.FindFirst(x => x.Type == "Id") != null)
                {
                    b = a?.FindFirst(x => x.Type == "Id").Value;
                }
                else
                {
                    if (context.Request.Path.Value != "/api/v1/Authentication/ForgotPassword")
                    {
                        var token = CommonMethods.GetTokenFromResponseData(responseData);
                        if (token != null && token != "")
                        {
                            var claims = CommonMethods.ExtractClaims(token);
                            b = (new ClaimsIdentity(claims)).Claims.FirstOrDefault(x => x.Type == "Id").Value;
                        }
                    }

                }

                _context.ActivityLogs.Add(new ActivityLog
                {

                    Ipaddress = context.Request.Method,
                    LogDatetime = DateTime.Now,
                    RequestBody = requestData,
                    RequestHost = context.Request.Host.Value,
                    RequestMethod = context.Request.Method,
                    RequestScheme = context.Request.Scheme,
                    RequestUrl = context.Request.Path,
                    ResponseBody = responseData,
                    ResponseMessage = responseData,
                    ResponseStatus = Convert.ToString(context.Response.StatusCode),
                    UserId = b
                });

                _context.SaveChanges();






            }



            //TODO: Save log to chosen datastore


            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            requestData = bodyAsText;
            // reset the stream position to 0, which is allowed because of EnableBuffering()
            request.Body.Seek(0, SeekOrigin.Begin);
            query = request.QueryString;
            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private static async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            responseData = text;
            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}
