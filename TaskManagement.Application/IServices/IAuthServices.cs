using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Response;

namespace TaskManagement.Application.IServices
{
    public interface IAuthServices
    {
        Task<Response<bool>> Register(RegisterDTO registerDTO);
        Task<Response<LoginResponseDTO>> Login(LoginDTO loginDTO);
        Task<Response<string>> RefreshToken(string oldToken, string refreshToken);




        Task<Response<UserProfileDTO>> GetUserProfileAsync();


        Task<int?> GetCurrentUserId();


    }
}
