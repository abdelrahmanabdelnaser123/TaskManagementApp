using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Dtos
{
    public class LoginDTO
    {
        [DefaultValue("admin1@example.com")]
        public string Email { get; set; }
        [DefaultValue("Admin@123")]
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string RefreshToken { get; set; }
    }
}
