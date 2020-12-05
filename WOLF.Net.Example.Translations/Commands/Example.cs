using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Enums.Subscribers;
using WOLF.Net.Example.Entities;
using WOLF.Net.Utilities;

namespace WOLF.Net.ExampleBot.Commands
{

    [CommandCollection("example"), RequiredMessageType(MessageType.Group)]
    class Example : CommandContext
    {
        private Cache Cache => Program.Cache;

        [Command]
        public async Task Default1() => await Help();

        [Command("help")]
        public async Task Help() => await ReplyAsync(Bot.GetPhraseByName(Command.Language, "help_message"));

        [Command("start")]
        public async Task Start()
        {
            if (await Cache.ExistsAsync(Command.SourceTargetId))
                await ReplyAsync(Bot.GetPhraseByName(Command.Language, "flow_exists_message"));
            else
            {
                await Cache.SetAsync(Message.SourceTargetId, new FormData(Command.SourceTargetId, Command.SourceSubscriberId, Command.Language));

                await ReplyAsync(Bot.GetPhraseByName(Command.Language, "send_age_message"));
            }
        }

        [Command("cancel"), RequiredPermissions(Capability.Admin, Privilege.STAFF)]
        public async Task Cancel()
        {
            if (!await Cache.ExistsAsync(Command.SourceTargetId))
                await ReplyAsync(Bot.GetPhraseByName(Command.Language, "no_flow_exists_message"));

            if (await Cache.DeleteAsync(Command.SourceTargetId))
                await ReplyAsync(Bot.GetPhraseByName(Command.Language, "flow_cancelled_message"));
        }

        [CommandCollection("get")]
        public class Get : CommandContext
        {
            [Command]
            public async Task Default() => await ReplyAsync(Bot.GetPhraseByName(Command.Language, "help_message"));

            [Command("subscriber"), RequiredPermissions(Enums.Groups.Capability.Mod, Privilege.STAFF)]
            public async Task Subscriber()
            {
                int subscriberId = Message.SourceSubscriberId;

                if (int.TryParse(Command.Argument.ToEnglishNumbers(), out int sid) && sid > 0)
                    subscriberId = sid;

                var subscriber = await Bot.GetSubscriberAsync(subscriberId);

                if (subscriber.Exists)
                {
                    var chunks = JsonConvert.SerializeObject(subscriber, Formatting.Indented).BatchString(1000);

                    foreach (var chunk in chunks)
                        await ReplyAsync(chunk);
                }
                else
                    await ReplyAsync(Bot.GetPhraseByName(Command.Language, "no_such_subscriber_exists_message"));
            }

            [Command("group"), RequiredPermissions(Enums.Groups.Capability.Mod, Privilege.STAFF)]
            public async Task Group()
            {
                int groupId = Message.SourceTargetId;

                if (int.TryParse(Command.Argument.ToEnglishNumbers(), out int gid) && gid > 0)
                    groupId = gid;

                var group = await Bot.GetGroupAsync(groupId);

                if (group.Exists)
                {
                    var chunks = JsonConvert.SerializeObject(group, Formatting.Indented).BatchString(1000);

                    foreach (var chunk in chunks)
                        await ReplyAsync(chunk);
                }
                else
                    await ReplyAsync(Bot.GetPhraseByName(Command.Language, "no_such_group_exists_message"));
            }
        }
    }
}