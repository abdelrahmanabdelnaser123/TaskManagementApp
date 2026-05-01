using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.IServices;
using TaskManagement.Application.Response;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthServices authServices) : ControllerBase
    {
        private readonly IAuthServices authServices = authServices;

        [HttpPost("login")]
        public async Task<Response<LoginResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            return await authServices.Login(loginDTO);
        }

        [HttpPost("register")]
        public async Task<Response<bool>> Register([FromBody] RegisterDTO registerDTO)
        {
            return await authServices.Register(registerDTO);
        }

        [HttpPost("refresh-token")]
        public async Task<Response<string>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            return await authServices.RefreshToken(refreshTokenDto.token, refreshTokenDto.refreshToken);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<Response<UserProfileDTO>> GetUserProfile()
        {
            return await authServices.GetUserProfileAsync();
        }
    }
}
