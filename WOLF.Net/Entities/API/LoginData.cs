using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.API;

namespace WOLF.Net.Entities.API
{
    public class LoginData
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public LoginDevice LoginDevice { get; set; }

        public LoginType LoginType { get; set; }

        public string Token { get; set; }

        public LoginData(string email, string password, LoginDevice loginDevice, LoginType loginType)
        {
            Email = email;
            Password = password;
            LoginDevice = loginDevice;
            LoginType = loginType;
        }
    }
}
