using System.Collections.Generic;
using System.Net;

namespace AuctionSystem.Domain.Common
{
    public class Response<T>
    {
        public Response()
        {
        }
        // Successful response
        public Response(T data, string message = "Success")
        {
            Succeeded = true;
            Code = 200;
            Message = message;
            Data = data;
        }

        // Successful response
        public Response(string message, int code, bool succeeded)
        {
            Succeeded = succeeded;
            Code = code;
            Message = message;
        }

        // Error response
        public Response(string message, bool succeeded, int code, List<string> errors = null)
        {
            Succeeded = succeeded;
            Code = code;
            Message = message;
            Errors = errors;
        }
        // Error list response
        public Response(string message, List<string> errors = null)
        {
            Succeeded = false;
            Code = (int)HttpStatusCode.BadRequest;
            Message = message;
            Errors = errors;
        }
        public bool Succeeded { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
