using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using MediaMarkup.Api.Models;
using Newtonsoft.Json;

namespace MediaMarkup
{
    public class ApiException : Exception
    {
        public ApiException(string reference, HttpStatusCode statusCode, string responseContent)
        {
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                {
                    throw new AuthenticationException();
                }
                case HttpStatusCode.BadRequest:
                {
                    var message = responseContent;
                    try
                    {
                        var errorResult = JsonConvert.DeserializeObject<ErrorResult>(responseContent);
                        message = errorResult.Message;
                        throw new Exception($"{(int)statusCode}, {statusCode}, {reference} : {responseContent}");
                    }
                    catch
                    {
                        throw new Exception($"{(int)statusCode}, {statusCode}, {reference} : {message}");
                    }
                }
                default:
                {
                    throw new Exception($"{(int)statusCode}, {statusCode}, {reference} : {responseContent}");
                }
            }
        }
    }
}
