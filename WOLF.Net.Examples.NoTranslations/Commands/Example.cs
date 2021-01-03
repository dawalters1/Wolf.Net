using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Enums.Subscribers;
using WOLF.Net.Utilities;

namespace WOLF.Net.ExampleBot.Commands
{

    [Command("!example")]
    class Example : CommandContext
    {
        [Command]
        public async Task Default1() => await Help();

        [Command("help")]
        public async Task Help() => await ReplyAsync("Welcome to the example bot\n\n!example help - Will display this message\n!example get subscriber <subscriberId> - get a subscriber profile\n!example get group <groupId> - get a group profile");

        [Command("cancel")]
        public async Task Cancel()
        {
            if (Command.IsGroup)
            {
                if (Bot.FormManager.HasGroupForm(Command.SourceTargetId, Command.SourceSubscriberId) ? Bot.FormManager.CancelGroupForm(Command.SourceTargetId, Command.SourceSubscriberId) : Bot.FormManager.CancelGroupForms(Command.SourceTargetId))
                    await ReplyAsync("(Y) Form Cancelled");
                else
                    await ReplyAsync("(N) Theres nothing to cancel");
            }
            else
            {
                if (Bot.FormManager.CancelPrivateForm(Command.SourceSubscriberId))
                    await ReplyAsync("(Y) Form Cancelled");
                else
                    await ReplyAsync("(N) Theres nothing to cancel");
            }
        }

        [Command("get")]
        public class Get : CommandContext
        {
            [Command]
            public async Task Default() => await ReplyAsync("Welcome to the example bot\n\n!example help - Will display this message\n!example get subscriber <subscriberId> - get a subscriber profile\n!example get group <groupId> - get a group profile");

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
                    await ReplyAsync("No such subscriber exists with this ID");
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
                    await ReplyAsync("No such group exists with this ID");
            }
        }
    }
}