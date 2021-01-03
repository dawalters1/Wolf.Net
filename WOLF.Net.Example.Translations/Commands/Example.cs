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

    [Command("example"), RequiredMessageType(MessageType.Group)]
    class Example : CommandContext
    {
        [Command]
        public async Task Default1() => await Help();

        [Command("help")]
        public async Task Help() => await ReplyAsync(Bot.GetPhraseByName(Command.Language, "help_message"));

        [Command("cancel"), RequiredPermissions(Capability.Admin, Privilege.STAFF)]
        public async Task Cancel()
        {
            if (Command.IsGroup)
            {
                if (Bot.FormManager.HasGroupForm(Command.SourceTargetId, Command.SourceSubscriberId)? Bot.FormManager.CancelGroupForm(Command.SourceTargetId, Command.SourceSubscriberId):Bot.FormManager.CancelGroupForms(Command.SourceTargetId))
                    await ReplyAsync(Bot.GetPhraseByName(Command.Language, "no_flow_exists_message"));
                else
                    await ReplyAsync(Bot.GetPhraseByName(Command.Language, "flow_cancelled_message"));
            }
            else
            {
                if (Bot.FormManager.CancelPrivateForm(Command.SourceSubscriberId))
                    await ReplyAsync(Bot.GetPhraseByName(Command.Language, "no_flow_exists_message"));
                else
                    await ReplyAsync(Bot.GetPhraseByName(Command.Language, "flow_cancelled_message"));
            }
        }

        [Command("get")]
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