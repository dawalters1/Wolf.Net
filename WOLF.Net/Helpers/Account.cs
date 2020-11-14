using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Contacts;
using WOLF.Net.Enums.Subscribers;
using WOLF.Net.Utilities;

namespace WOLF.Net
{
    //TO-DO: 
    /*
     * Update Profile
     * V2 support I guess?
     * GetJoinedGroups
     */
    public partial class WolfBot
    {
        public List<Contact> Contacts = new List<Contact>();

        internal async Task<Response<CurrentSubscriber>> InternalLoginAsync()
        {
            return await WolfClient.Emit<CurrentSubscriber>(Request.SECURITY_LOGIN, new
            {
                headers = new
                {
                    version = 2
                },
                body = new
                {
                    type = "email",
                    deviceTypeId = (int)LoginData.LoginDevice,
                    username = LoginData.Email,
                    password = LoginData.Password.ToMD5(),
                    md5Password = true
                }
            });
        }

        public async Task<Response> LogoutAsync()
        {
            return await WolfClient.Emit(Request.SECURITY_LOGOUT, new { });
        }

        public async Task<Response> SetOnlineStateAsync(OnlineState onlineState)
        {
            return await WolfClient.Emit(Request.MESSAGE_SETTING_UPDATE, new
            {
                state = new { state = onlineState } // State inside state? wtf is this shit...
            });
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            if (Contacts.Count > 0&&!Contacts.All(r=>r.IsBlocked))
                return Contacts.Where(r=>!r.IsBlocked).ToList();

            var result = await WolfClient.Emit<List<Contact>>(Request.SUBSCRIBER_CONTACT_LIST, new 
            { 
                subscribe = true
            });

            if(result.Success)
            {
                Contacts.AddRange(result.Body.Select(r=> { r.IsBlocked = false; return r; }).ToList());

                return Contacts.Where(r => !r.IsBlocked).ToList();
            }

            return new List<Contact>();
        }

        public async Task<List<Contact>> GetBlockedListAsync()
        {
            if (Contacts.Count > 0 && Contacts.Any(r => r.IsBlocked))
                return Contacts.Where(r => r.IsBlocked).ToList();

            var result = await WolfClient.Emit<List<Contact>>(Request.SUBSCRIBER_BLOCK_LIST, new
            {
                subscribe = true
            });

            if (result.Success)
            {
                Contacts.AddRange(result.Body.Select(r => { r.IsBlocked = true; return r; }).ToList());

                return Contacts.Where(r=>r.IsBlocked).ToList();
            }

            return new List<Contact>();
        }


        internal void ProcessContact(Contact contact)
        {
            if (Contacts.Any(r => r.Id == contact.Id))
                Contacts.FirstOrDefault(r => r.Id == contact.Id).Update(contact);
            else
                Contacts.Add(contact);
        }

    }
}
