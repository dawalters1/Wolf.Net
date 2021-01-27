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
		//public WolfClient Client { get; set; }

		public WolfBot Bot { get; set; }

		public CommandData Command { get; set; }

		public Message Message { get; set; }

		public async Task<Response<MessageResponse>> ReplyAsync(object content) => await SendMessageAsync(content);

		public async Task<Response<MessageResponse>> SendMessageAsync(object content) => await Bot.SendMessageAsync(Command.SourceTargetId, content, Command.MessageType);

		public async Task<Response<MessageResponse>> SendPrivateMessageAsync(int subscriberId, object content) => await Bot.SendPrivateMessageAsync(subscriberId, content);

		public async Task<Response<MessageResponse>> SendGroupMessageAsync(int targetGroupId, object content) => await Bot.SendGroupMessageAsync(targetGroupId, content);
    }
}
