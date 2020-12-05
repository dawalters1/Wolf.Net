using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Example.Entities;
using WOLF.Net.Utilities;

namespace WOLF.Net.ExampleBot.Flows
{
    public static class FormExample
    {
        private static async Task AgeInput(WolfBot bot, Message message, Cache cache, FormData formData)
        {
            if (int.TryParse(message.Content.ToEnglishNumbers(), out int age))
                if (age <= 0)
                    await bot.SendGroupMessageAsync(message.SourceTargetId, "I find it highly unlikely that you are that young");
                else if (age >= 100)
                    await bot.SendGroupMessageAsync(message.SourceTargetId, "I find it highly unlikely that you are that old");
                else
                {
                    formData.Stage += 1;
                    formData.Data.Age = age;
                    await cache.SetAsync(message.SourceTargetId, formData, TimeSpan.FromSeconds(60));

                    await bot.SendGroupMessageAsync(message.SourceTargetId, "Alright then, what is your name?");
                }
            else
                await bot.SendGroupMessageAsync(message.SourceTargetId, "That is not a number!");
        }

        private static async Task NameInput(WolfBot bot, Message message, Cache cache, FormData formData)
        {
            await cache.DeleteAsync(message.SourceTargetId);

            await bot.SendGroupMessageAsync(message.SourceTargetId, string.Format("You are {0} years old and your name is {1}", formData.Data.Age, message.Content));
        }

        public static async Task Handle(WolfBot bot, Message message, Cache cache, FormData formData)
        {
            switch (formData.Stage)
            {
                case 1:
                    await AgeInput(bot, message, cache, formData);
                    break;
                case 2:
                    await NameInput(bot, message, cache, formData);
                    break;
            }
        }
    }
}