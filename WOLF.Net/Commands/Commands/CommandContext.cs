using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Entities.Subscribers;

namespace WOLF.Net.Commands.Commands
{
    public abstract class CommandContext
    {
		public WolfBot Bot { get; set; }

		public CommandData Command { get; set; }

		public Message Message { get; set; }

		public async Task<Response<MessageResponse>> ReplyAsync(object content, bool includeEmbeds = false) => await SendMessageAsync(content, includeEmbeds);

		public async Task<Response<MessageResponse>> SendMessageAsync(object content, bool includeEmbeds = false) => await Bot.Messaging().SendMessageAsync(Message.IsGroup? Message.TargetGroupId: Message.SourceSubscriberId, content, Message.MessageType, includeEmbeds);

		public async Task<Response<MessageResponse>> SendPrivateMessageAsync(int subscriberId, object content, bool includeEmbeds = false) => await Bot.Messaging().SendPrivateMessageAsync(subscriberId, content, includeEmbeds);

		public async Task<Response<MessageResponse>> SendGroupMessageAsync(int targetGroupId, object content, bool includeEmbeds = false) => await Bot.Messaging().SendGroupMessageAsync(targetGroupId, content, includeEmbeds);
    }
}
