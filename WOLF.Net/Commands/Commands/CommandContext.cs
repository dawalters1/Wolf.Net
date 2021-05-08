using System.Threading.Tasks;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;

namespace WOLF.Net.Commands.Commands
{
    public abstract class CommandContext
    {
		public WolfBot Bot { get; set; }

		public CommandData Command { get; set; }

		public Message Message { get; set; }

		public async Task<Response<MessageResponse>> ReplyAsync(object content, bool includeEmbeds = false) => await SendMessageAsync(content, includeEmbeds);

		public async Task<Response<MessageResponse>> SendMessageAsync(object content, bool includeEmbeds = false) => await Bot.SendMessageAsync(Message.IsGroup? Message.TargetGroupId: Message.SourceSubscriberId, content, Message.MessageType, includeEmbeds);

		public async Task<Response<MessageResponse>> SendPrivateMessageAsync(int subscriberId, object content, bool includeEmbeds = false) => await Bot.SendPrivateMessageAsync(subscriberId, content, includeEmbeds);

		public async Task<Response<MessageResponse>> SendGroupMessageAsync(int targetGroupId, object content, bool includeEmbeds = false) => await Bot.SendGroupMessageAsync(targetGroupId, content, includeEmbeds);
    }
}
