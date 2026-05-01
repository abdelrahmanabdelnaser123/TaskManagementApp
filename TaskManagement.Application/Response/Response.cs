using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Response
{
    public sealed class Response<T> : IResponse<T>
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; }
        public T Data { get; set; }
        public int SubStatus { get; set; }

        public Response() { }

        public Response(ResponseStatus status, T data, string message = null, string internalMessage = null, int subStatus = 0)
        {
            Status = status;
            Data = data;
            Message = message;
            InternalMessage = internalMessage;
            SubStatus = subStatus;
        }


        public static Response<T> Success(T data, string message = null)
        {
            return new Response<T>(ResponseStatus.Success, data, message);
        }


        public static Response<T> Fail(string message, ResponseStatus status = ResponseStatus.BadRequest, int subStatus = 0, string internalMessage = null)
        {
            return new Response<T>
            {
                Status = status,
                Message = message,
                InternalMessage = internalMessage,
                SubStatus = subStatus,
                Data = default
            };
        }


        public static Response<T> Error(string message, string internalMessage = null, int subStatus = 0)
        {
            return new Response<T>
            {
                Status = ResponseStatus.InternalServerError,
                Message = message,
                InternalMessage = internalMessage,
                SubStatus = subStatus,
                Data = default
            };
        }
    }
}
