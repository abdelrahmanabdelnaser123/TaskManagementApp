using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Enums
{
    public enum ResponseStatus
    {


        Success = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,

        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        TooManyRequests = 429,

        InternalServerError = 500,
        NotImplemented = 501,
        ServiceUnavailable = 503,
    }
    public enum CustomeResponse
    {
        Success = 0,
        Error = 1,
        AuthFailure = 2,
        Conflict = 3,
    }
}
