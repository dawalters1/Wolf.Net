using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Form;

namespace Wolf.Net.Example.Forms
{
    [Form("example_form")]
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

            OnCancel += async () => await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_cancelled_message"), Command.Subscriber.ToDisplayName(trimAds: true)));
            OnTimeout += async () => await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_timeout_message"), Command.Subscriber.ToDisplayName(trimAds: true)));

            await SendMessageAsync(Bot.Phrase().GetByName(Command.Language, "example_send_age_message"));

            NextStage(AgeInput);
        }

        public async void AgeInput(string message)
        {
            ChangeTimeoutDelay(10000);

            if (int.TryParse(message.ToEnglishNumbers(), out int age))
                if (age <= 0)
                    await SendMessageAsync(Bot.Phrase().GetByName(Command.Language, "example_form_error_doubt_you_are_that_young_message"));
                else if (age >= 100)
                    await SendMessageAsync(Bot.Phrase().GetByName(Command.Language, "example_form_error_doubt_you_are_that_old_message"));
                else
                {
                    this.age = age;
                    NextStage(NameInput);
                    await SendMessageAsync(Bot.Phrase().GetByName(Command.Language, "example_send_name_message"));
                }
            else
                await SendMessageAsync(Bot.Phrase().GetByName(Command.Language, "example_form_error_invalid_number_message"));
        }

        public async void NameInput(string message)
        {
            await SendMessageAsync(string.Format(Bot.Phrase().GetByName(Command.Language, "example_details_message"), age, message));

            Finish();
        }
    }
}