using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TaskManagement.Application.AuthHelper;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.IServices;
using TaskManagement.Application.Response;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services
{
    public class AuthServices(
     UserManager<ApplicationUser> userManager,
     IConfiguration config,
     IUnitOfWork unitOfWork,
     IHttpContextAccessor httpContextAccessor) : IAuthServices
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IConfiguration config = config;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        public IUnitOfWork UnitOfWork { get; } = unitOfWork;

        public async Task<Response<bool>> Register(RegisterDTO registerDTO)
        {
            // Check if user exists
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return new Response<bool>
                {
                    Data = false,
                    Status = ResponseStatus.Conflict,
                    Message = "User already exists"
                };
            }

            var newUser = new ApplicationUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                Name = registerDTO.Name,
                PhoneNumber = registerDTO.PhoneNumber,
            };

            // Create user
            var creationResult = await userManager.CreateAsync(newUser, registerDTO.Password);
            if (!creationResult.Succeeded)
            {
                return new Response<bool>
                {
                    Data = false,
                    Status = ResponseStatus.BadRequest,
                    Message = "User registration failed",
                    InternalMessage = string.Join(", ", creationResult.Errors.Select(e => e.Description))
                };
            }

            // Assign role 
            var roleName = registerDTO.role.ToString();

            var roleResult = await userManager.AddToRoleAsync(newUser, roleName);
            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(newUser);

                return new Response<bool>
                {
                    Data = false,
                    Status = ResponseStatus.BadRequest,
                    Message = "User registration failed",
                    InternalMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description))
                };
            }

            await AddRoleSpecificEntityAsync(newUser.Id, registerDTO.role);

            return new Response<bool>
            {
                Data = true,
                Status = ResponseStatus.Success,
                Message = "User registered successfully"
            };
        }



        public async Task<Response<LoginResponseDTO>> Login(LoginDTO loginDTO)
        {
            try
            {

                // Check if the user exists
                ApplicationUser? user = await userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    return new Response<LoginResponseDTO>
                    {
                        Data = null,
                        Status = ResponseStatus.NotFound,
                        Message = "User not found"
                    };
                }

                // Check if the password is correct
                bool result = await userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (result)
                {
                    string token = await userManager.GenerateTokenAsync(user);
                    string refreshToken = await userManager.GenerateRefreshTokenAsync(user);
                    return new Response<LoginResponseDTO>
                    {
                        Data = new LoginResponseDTO
                        {
                            Token = token,
                            RefreshToken = refreshToken,
                            Name = user.Name,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber
                        },
                        Status = ResponseStatus.Success,
                        Message = "Login Success"
                    };
                }

                return new Response<LoginResponseDTO>
                {
                    Data = null,
                    Status = ResponseStatus.BadRequest,
                    Message = "Invalid Email or Password",

                };
            }
            catch (Exception ex)
            {
                return new Response<LoginResponseDTO>
                {
                    Data = null,
                    Status = ResponseStatus.InternalServerError,
                    Message = "An error occurred during login",
                    InternalMessage = ex.Message
                };
            }


        }
        public async Task<Response<string>> RefreshToken(string oldToken, string refreshToken)
        {
            try
            {
                string? token = await userManager.RefreshToken(oldToken, refreshToken);

                if (token == null)
                {
                    return new Response<string>
                    {
                        Data = null,
                        Status = ResponseStatus.BadRequest,
                        Message = "Invalid refresh token or token expired"
                    };
                }

                return new Response<string>
                {
                    Data = token,
                    Status = ResponseStatus.Success,
                    Message = "Token refreshed successfully"
                };
            }
            catch (Exception ex)
            {
         

                return new Response<string>
                {
                    Data = null,
                    Status = ResponseStatus.InternalServerError,
                    Message = $"An error occurred while refreshing the token: {ex.Message}"
                };
            }
        }



        public async Task<Response<UserProfileDTO>> GetUserProfileAsync()
        {
            var userId = await GetCurrentUserId();

            if (userId == null)
            {
                return new Response<UserProfileDTO>
                {
                    Status = ResponseStatus.Unauthorized,
                    Message = "User not authenticated"
                };
            }

            ApplicationUser? user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return new Response<UserProfileDTO>
                {
                    Status = ResponseStatus.NotFound,
                    Message = "User not found"
                };
            }

            return new Response<UserProfileDTO>
            {
                Data = new UserProfileDTO
                {
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                },
                Status = ResponseStatus.Success,
                Message = "User profile retrieved successfully"
            };
        }


        private Task AddRoleSpecificEntityAsync(int userId, Role role)
        {
            switch (role)
            {
                case Role.Admin:
                    break;

                case Role.User:
                    break;
            }

            return Task.CompletedTask;
        }
        public async Task<int?> GetCurrentUserId()
        {
            var principal = httpContextAccessor.HttpContext?.User;

            var id = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(id, out var userId) ? userId : null;
        }


    }
}
