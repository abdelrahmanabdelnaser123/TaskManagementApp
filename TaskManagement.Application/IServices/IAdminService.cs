using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Response;

namespace TaskManagement.Application.IServices
{
    public interface IAdminService
    {
        Task<Response<List<UserDto>>> GetUsers();
        Task<Response<bool>> CreateUser(CreateUserByAdminDto dto);
        Task<Response<bool>> DeleteUser(string userId);
    }
}
