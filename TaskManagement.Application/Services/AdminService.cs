using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.IServices;
using TaskManagement.Application.Response;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<List<UserDto>>> GetUsers()
        {
            var users = _userManager.Users.ToList();

            return new Response<List<UserDto>>
            {
                Data = users.Select(u => new UserDto
                {
                    Id = u.Id.ToString(),
                    Name = u.Name,
                    Email = u.Email!
                }).ToList(),
                Status = ResponseStatus.Success
            };
        }

        public async Task<Response<bool>> CreateUser(CreateUserByAdminDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new Response<bool>
                {
                    Data = false,
                    Status = ResponseStatus.BadRequest,
                    Message = string.Join(",", result.Errors.Select(e => e.Description))
                };
            }

            await _userManager.AddToRoleAsync(user, "User");

            return new Response<bool>
            {
                Data = true,
                Status = ResponseStatus.Success,
                Message = "User created successfully"
            };
        }

        public async Task<Response<bool>> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new Response<bool>
                {
                    Status = ResponseStatus.NotFound,
                    Message = "User not found"
                };
            }

            await _userManager.DeleteAsync(user);

            return new Response<bool>
            {
                Data = true,
                Status = ResponseStatus.Success,
                Message = "User deleted successfully"
            };
        }
    }
}