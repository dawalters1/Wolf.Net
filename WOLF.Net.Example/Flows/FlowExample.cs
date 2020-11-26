using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Example.Entities;
using WOLF.Net.Utilities;

namespace WOLF.Net.ExampleBot.Flows
{
    public static class FlowExample
    {
        private static async Task AgeInput(WolfBot bot, Message message, Cache cache, FlowData flowData)
        {
            if (int.TryParse(message.Content.ToEnglishNumbers(), out int age))
                if (age <= 0)
                    await bot.SendGroupMessageAsync(message.SourceTargetId, bot.GetPhraseByName(flowData.Language, "doubt_you_are_that_young_message"));
                else if (age >= 100)
                    await bot.SendGroupMessageAsync(message.SourceTargetId, bot.GetPhraseByName(flowData.Language, "doubt_you_are_that_old_message"));
                else
                {
                    flowData.Stage += 1;
                    flowData.Data.Age = age;
                    await cache.SetAsync(message.SourceTargetId, flowData, TimeSpan.FromSeconds(60));

                    await bot.SendGroupMessageAsync(message.SourceTargetId, bot.GetPhraseByName(flowData.Language, "send_name_message"));
                }
            else
                await bot.SendGroupMessageAsync(message.SourceTargetId, bot.GetPhraseByName(flowData.Language, "invalid_number_message"));
        }

        private static async Task NameInput(WolfBot bot, Message message, Cache cache, FlowData flowData)
        {
            await cache.DeleteAsync(message.SourceTargetId);

            await bot.SendGroupMessageAsync(message.SourceTargetId, string.Format(bot.GetPhraseByName(flowData.Language, "details_message"), flowData.Data.Age, message.Content));
        }

        public static async Task Handle(WolfBot bot, Message message, Cache cache, FlowData flowData)
        {
            switch (flowData.Stage)
            {
                case 1:
                    await AgeInput(bot, message, cache, flowData);
                    break;
                case 2:
                    await NameInput(bot, message, cache, flowData);
                    break;
            }
        }
    }
}