using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Form;

namespace Wolf.Net.Example.Forms
{
    [Form("example_command_example_form")]
    class Example : FormContext
    {
        public int age { get; set; }

        public override async void Start(string message)
        {
            if (Bot.FormManager.GroupHasForms(Command.SourceTargetId, Command.SourceTargetId))//Ensure to include the current subscriber Id this will exclude them from the count, else this will always throw true
            {
                Finish();

                await SendMessageAsync(Bot.Phrase().GetByName(Command.Language, "example_form_error_exists_message"));

                return;
            }

            OnTimeout += async () => await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_timeout_message"), Command.Subscriber.ToDisplayName(trimAds: true)));

            await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_send_age_message"), Command.Subscriber.ToDisplayName(trimAds: true)));

            NextStage(AgeInput);
        }

        public async void AgeInput(string message)
        {
            ChangeTimeoutDelay(10000);

            if (int.TryParse(message.ToEnglishNumbers(), out int age))
                if (age <= 0)
                    await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_form_error_doubt_you_are_that_young_message", Command.Subscriber.ToDisplayName(trimAds: true)));
                else if (age >= 100)
                    await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_form_error_doubt_you_are_that_old_message", Command.Subscriber.ToDisplayName(trimAds: true)));
                else
                {
                    this.age = age;
                    NextStage(NameInput);
                    await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_send_name_message", Command.Subscriber.ToDisplayName(trimAds: true)));
                }
            else
                await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_form_error_invalid_number_message", Command.Subscriber.ToDisplayName(trimAds: true)));
        }

        public async void NameInput(string message)
        {
            await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_details_message"), age, message, Command.Subscriber.ToDisplayName(trimAds: true)));

            Finish();
        }
    }
}