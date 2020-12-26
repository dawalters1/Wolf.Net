using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Enums.API;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.API
{
    public class LoginData
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public LoginDevice LoginDevice { get; set; }

        public int LoginDeviceId => (int)LoginDevice;

        public LoginType LoginType { get; set; }

        public OnlineState OnlineState { get; set; }

        public string Token { get; }

        public Cognito Cognito { get; set; }

        public LoginData(string email, string password, LoginDevice loginDevice, LoginType loginType, OnlineState onlineState)
        {
            Email = email;
            Password = password;
            LoginDevice = loginDevice;
            LoginType = loginType;
            OnlineState = onlineState;
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
