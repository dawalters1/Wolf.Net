using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Contacts;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net
{
    //TO-DO: 
    /*
     * Login
     * Logout
     * Set online state
     * Update Profile
     * V2 support I guess?
     * Get Contacts
     * Get Blocked 
     * 
     */
    public partial class WolfBot
    {
        public async Task<Response<CurrentSubscriber>> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Response> SetOnlineStateAsync(OnlineState onlineState)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Contact>> GetBlockedListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
