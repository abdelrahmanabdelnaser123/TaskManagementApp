using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Response
{
    public interface IResponse<T>
    {
        ResponseStatus Status { get; set; }

        int SubStatus { get; set; }

        string Message { get; set; }

        string InternalMessage { get; set; }

        T Data { get; set; }

    }
}
