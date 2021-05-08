using Newtonsoft.Json;
using SocketIOClient.Newtonsoft.Json;

namespace WOLF.Net.Networking
{
    public class CustomJsonSerializer : NewtonsoftJsonSerializer
    {
        public CustomJsonSerializer(int eio) : base(eio) { }

        public override JsonSerializerSettings CreateOptions()
        {
            return new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
    }
}