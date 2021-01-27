using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Commands.Form;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Utilities;

namespace WOLF.Net.ExampleBot.Flows
{
    [Form("!example form", 5000), RequiredMessageType(Enums.Messages.MessageType.Group)]
    public class ExampleForm : FormContext
    {
        public int Age { get; set; }

        public override async void Start(string message)
        {
            if (Bot.FormManager.GroupHasForms(Command.SourceTargetId))
            {
                Finish();

                await ReplyAsync("Someone else has already started a form in this group");

                return;
            }

            OnTimeout += async () => await ReplyAsync($"(N) {Command.Subscriber.ToDisplayName(trimAds:true)} you failed to complete the form in time\nSend !example form to try again");

            await ReplyAsync("How old are you?");

            NextStage(AgeInput);
        }

        public async void AgeInput(string message)
        {
            ChangeTimeoutDelay(10000);

            if (int.TryParse(message.ToEnglishNumbers(), out int age))
                if (age <= 0)
                    await ReplyAsync("I find it highly unlikely that you are that young");
                else if (age >= 100)
                    await ReplyAsync("I find it highly unlikely that you are that old");
                else
                {
                    Age = age;
                    NextStage(NameInput);
                    await ReplyAsync("Alright then, what is your name?");
                }
            else
                await ReplyAsync("That is not a number!");
        }

        public async void NameInput(string message)
        {
            await ReplyAsync(string.Format("You are {0} years old and your name is {1}", Age, message));

            Finish();
        }
    }
}