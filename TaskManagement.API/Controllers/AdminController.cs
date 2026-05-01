using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.IServices;
using TaskManagement.Application.Response;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<Response<List<UserDto>>> GetUsers()
        {
            return await _adminService.GetUsers();
        }

        [HttpPost("users")]
        public async Task<Response<bool>> CreateUser(CreateUserByAdminDto dto)
        {
            return await _adminService.CreateUser(dto);
        }

        [HttpDelete("users/{id}")]
        public async Task<Response<bool>> DeleteUser(string id)
        {
            return await _adminService.DeleteUser(id);
        }
    }
}