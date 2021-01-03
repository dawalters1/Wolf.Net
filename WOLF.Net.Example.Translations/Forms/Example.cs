using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Commands.Form;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Example.Entities;
using WOLF.Net.Utilities;

namespace WOLF.Net.ExampleBot.Flows
{
    [Form("example_form", 5000), RequiredMessageType(Enums.Messages.MessageType.Group)]
    public class ExampleForm : FormContext
    {
        public int Age { get; set; }

        public override async void Start(string message)
        {
            if (Bot.FormManager.GroupHasForms(Command.SourceTargetId))
            {
                Finish();

                await ReplyAsync(Bot.GetPhraseByName(Command.Language, "form_exists_message"));

                return;
            }

            OnTimeout += async () => await ReplyAsync(string.Format(Bot.GetPhraseByName(Command.Language, "form_timeout_message"), Command.Subscriber.ToDisplayName(trimAds:true)));

            await ReplyAsync(Bot.GetPhraseByName(Command.Language, "send_age_message"));

            NextStage(AgeInput);
        }

        public async void AgeInput(string message)
        {
            ChangeTimeoutDelay(10000);

            if (int.TryParse(message.ToEnglishNumbers(), out int age))
                if (age <= 0)
                    await ReplyAsync(Bot.GetPhraseByName(Command.Language, "doubt_you_are_that_young_message"));
                else if (age >= 100)
                    await ReplyAsync(Bot.GetPhraseByName(Command.Language, "doubt_you_are_that_old_message"));
                else
                {
                    Age = age;
                    NextStage(NameInput);
                    await ReplyAsync(Bot.GetPhraseByName(Command.Language, "send_name_message"));
                }
            else
                await ReplyAsync(Bot.GetPhraseByName(Command.Language, "invalid_number_message"));
        }

        public async void NameInput(string message)
        {
            await ReplyAsync(string.Format(Bot.GetPhraseByName(Command.Language, "details_message"), Age, message));

            Finish();
        }
    }
}