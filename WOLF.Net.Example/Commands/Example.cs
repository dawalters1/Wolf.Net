using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Entities.Tip;
using WOLF.Net.Enums.Subscribers;

namespace Wolf.Net.Example.Commands
{
    [Command("example_command_example")]
    class Example : CommandContext
    {
        [Command]
        public async Task Default() => await SendMessageAsync(Bot.Phrase().GetByName(Command.Language, "example_help_message"));

        [Command("example_command_help")]
        public async Task Help() => await Default();

        [Command("example_command_hello")]
        public async Task Hello() => await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_hello_message"), Command.Subscriber.ToDisplayName(trimAds: true)));

        [Command("example_command_embed")]
        public async Task Embed() => await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_link_message")), true);

        [Command("example_command_image")]
        public async Task Image() => await SendMessageAsync(await "https://i.imgur.com/fuwb6KH.jpg".DownloadImageFromUrl());

        [Command("example_command_tip")]
        public async Task Tip() => await Bot.Tip().AddTip(Message, new TipCharm(594, 1));

        [Command("example_command_join"), RequiredPermissions(Privilege.STAFF)]
        public async Task Join()
        {
            if (string.IsNullOrWhiteSpace(Command.Argument))
                await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_join_error_provide_arguments_message"), Command.Subscriber.ToDisplayName(trimAds: true)));

            var arguments = Command.Argument.Split(new char[] { ',' });

            var result = await Bot.Group().JoinAsync(arguments.FirstOrDefault(), arguments.LastOrDefault());

            if (result.Success)
                await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_join_success_message"), Command.Subscriber.ToDisplayName(trimAds: true)));
            else
                await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_join_error_failed_message"), Command.Subscriber.ToDisplayName(trimAds: true), result.Headers != null ? result.Headers.ContainsKey("message") ? result.Headers["message"] : result.Headers["subCode"] : "unknown_reason"));
        }

        [Command("example_command_cancel")]
        public async Task Cancel()
        {
            if (Command.IsGroup)
            {
                if (Bot.FormManager.HasGroupForm(Command.TargetGroupId, Command.SourceSubscriberId) ? Bot.FormManager.CancelGroupForm(Command.TargetGroupId, Command.SourceSubscriberId) : Bot.FormManager.CancelGroupForms(Command.TargetGroupId))
                    await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_form_cancelled_message"), Command.Subscriber.ToDisplayName(trimAds: true)));
                else
                await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_error_no_form_exists_message"), Command.Subscriber.ToDisplayName(trimAds: true)));
            }
            else
            {
                if (Bot.FormManager.CancelPrivateForm(Command.SourceSubscriberId))
                    await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_form_cancelled_message"), Command.Subscriber.ToDisplayName(trimAds: true)));
                else
                    await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_error_no_form_exists_message"), Command.Subscriber.ToDisplayName(trimAds: true)));
               
            }
        }
    }
}