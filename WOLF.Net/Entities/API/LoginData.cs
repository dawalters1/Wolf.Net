using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Enums.API;

namespace WOLF.Net.Entities.API
{
    public class LoginData
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public LoginDevice LoginDevice { get; set; }

        public int LoginDeviceId => (int)LoginDevice;

        public LoginType LoginType { get; set; }

        public string Token { get; }

        public LoginData(string email, string password, LoginDevice loginDevice, LoginType loginType)
        {
            Email = email;
            Password = password;
            LoginDevice = loginDevice;
            LoginType = loginType;
            Token = GenerateToken();
        }

        private string GenerateToken()
        {
            string chars = "abcdefghi1234567890";

            StringBuilder token = new StringBuilder("WDN");

            while (token.Length < 35)
                token.Append(chars.OrderBy(r => Guid.NewGuid()).FirstOrDefault());

            return token.ToString().Trim();
        }
    }
}
