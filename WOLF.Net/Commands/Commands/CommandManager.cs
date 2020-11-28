﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Commands.Instances;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Utilities;

namespace WOLF.Net.Commands.Commands
{
    public class CommandManager
    {
        private readonly WolfBot Bot;

        //House the CommandCollection and its Children
        private List<TypeInstance<CommandCollection>> collections = new List<TypeInstance<CommandCollection>>();

        public CommandManager(WolfBot bot)
        {
            Bot = bot;
        }

        internal void Load()
        {
            if (collections.Count > 0)
                return;

            collections = typeof(CommandContext).GetAllTypes().Where(t => Attribute.IsDefined(t, typeof(CommandCollection))).Select(t => new TypeInstance<CommandCollection>(t, t.GetCustomAttribute<CommandCollection>())).ToList();

            foreach (var collection in collections)
                CheckCollection(collection);

            var duplicateCollections = collections.GroupBy(r => r.Value.Trigger.ToLower(), r => r.Value, (trigger, content) => new { trigger, commands = content.SelectMany(r => r.Commands).ToList(), subCollections = content.SelectMany(r=>r.SubCollections).ToList() }).ToList();

            foreach (var duplicate in duplicateCollections)
            {
                if (duplicate.commands.Count(r => string.IsNullOrWhiteSpace(r.Value.Trigger)) > 1)
                    throw new Exception($"You can only have 1 default command in collection {duplicate.trigger}");

                if(duplicate.commands.GroupBy(r=>r.Value.Trigger).Any(r=>r.Count()>1))
                    throw new Exception($"You can not have 2 of the same commands triggers in collection {duplicate.trigger}");

                if (duplicate.subCollections.GroupBy(r => r.Value.Trigger).Any(r => r.Count() > 1))
                    throw new Exception($"You can not have 2 of the same SubCommandCollections triggers in collection {duplicate.trigger}");
            }
        }

        private void CheckCollection(TypeInstance<CommandCollection> collection)
        {
            if (Bot.GetAllPhrasesByName(collection.Value.Trigger).Count == 0)
                throw new Exception($"Missing translation key {collection.Value.Trigger}"); 

            collection.Value.Commands = collection.Type.GetMethods().Where(t => Attribute.IsDefined(t, typeof(Command))).Select(t => new MethodInstance<Command>(t, t.GetCustomAttribute<Command>())).ToList();

            collection.Value.SubCollections = collection.Type.GetNestedTypes().Where(t => Attribute.IsDefined(t, typeof(CommandCollection))).Select(t => new TypeInstance<CommandCollection>(t, t.GetCustomAttribute<CommandCollection>())).ToList();

            if (collection.Value.Commands.Where(r=>!string.IsNullOrWhiteSpace(r.Value.Trigger)).Any(command => collection.Value.SubCollections.Any(s=>s.Value.Trigger.IsEqual(command.Value.Trigger))))
                throw new Exception($"You have commands sharing the same trigger as subcollections in class {collection.Type.Name}");

            foreach (var subCollection in collection.Value.SubCollections)
            {
                if (subCollection.Value.Commands.Any(command => subCollection.Value.Trigger.IsEqual(command.Value.Trigger)))
                    throw new Exception($"Commands cannot have the same trigger as collection {collection.Value.Trigger}");

                CheckCollection(subCollection);
            }
        }

        internal bool IsCommand(Message message) => collections.Any(r => Bot.GetAllPhrasesByName(r.Value.Trigger).Any(s=>s.Value.IsEqual(message.Content.Split(' ')[0])));

        private async void ExecuteCommand(Type type, MethodInstance<Command> command, Message message, CommandData commandData)
        {

            foreach (var attrib in command.Attributes)
                if (!await attrib.Validate(Bot, commandData))
                    return;

            var i = (CommandContext)Activator.CreateInstance(type);

            i.Command = commandData;
            i.Bot = Bot;
            i.Message = message;

            command.Type.Invoke(i, null);
        }

        private bool CheckCollection(TypeInstance<CommandCollection> collection, Message message, CommandData commandData)
        {
            var cmdArg = commandData.Argument.Split(' ')[0];

            foreach (var subCollection in collection.Value.SubCollections)
            {
                var phrase = Bot.GetPhraseByName(commandData.Language, subCollection.Value.Trigger);

                if (phrase == null||!phrase.IsEqual(cmdArg))
                    continue;

                commandData.Argument = string.Join(' ', commandData.Argument.Split(' ').Skip(1));

                CheckCollection(subCollection, message, commandData);

                return true;
            }

            foreach (var command in collection.Value.Commands.Where(r=>!string.IsNullOrWhiteSpace(r.Value.Trigger)).ToList())
            {
                var phrase = Bot.GetPhraseByName(commandData.Language, command.Value.Trigger);

                if (phrase == null || !phrase.IsEqual(cmdArg))
                    continue;

                commandData.Argument = string.Join(' ', commandData.Argument.Split(' ').Skip(1));

                ExecuteCommand(collection.Type, command, message, commandData);

                return true;
            }

            return false;
        }

        internal async Task ProcessMessage(Message message)
        {
            if (message.ContentType != ContentType.Text)
                return;

            var fixedArgs = string.Join(' ', message.Content.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries));

            var cmdArg = fixedArgs.Split(' ')[0];

            foreach (var collection in collections)
            {
                var phrase = Bot.GetAllPhrasesByName(collection.Value.Trigger).FirstOrDefault(r => r.Value.IsEqual(cmdArg));

                if (phrase!=null)
                {
                    var commandData = new CommandData()
                    {
                        Subscriber = await Bot.GetSubscriberAsync(message.SourceSubscriberId),
                        Group = message.IsGroup ? await Bot.GetGroupAsync(message.SourceTargetId) : null,
                        IsGroup = message.IsGroup,
                        Argument = string.Join(' ',fixedArgs.Split(' ').Skip(1)),
                        Language = phrase.Language,
                        MessageType = message.MessageType
                    };

                    foreach (var attrib in collection.Attributes)
                        if (!await attrib.Validate(Bot, commandData))
                            return;

                    if (CheckCollection(collection, message, commandData))
                        break;
                }
                else
                    continue;
            }
        }
    }
}