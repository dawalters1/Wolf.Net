using Newtonsoft.Json;
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
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Enums.Subscribers;
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

        private TypeInstance<CommandCollection> CreateCollection(TypeInstance<CommandCollection> col)
        {
            var trigger = col.Value.Trigger;

            var messageType = col.Type.GetMessageTypeOrDefault();
            var requiredPermissions = col.Type.GetRequiredCapabilityOrDefault();
            var requiredPrivileges = col.Type.GetRequiredPrivilegesOrDefault();
            var authOnly = col.Type.GetIsAuthOnly();

            var childrenCommands = col.Type.GetMethods().Where(method => Attribute.IsDefined(method, typeof(Command))).Select(t =>
            {
                var command = new MethodInstance<Command>(t, t.GetCustomAttribute<Command>());
                command.Value.MessageType = t.GetMessageTypeOrDefault();
                command.Value.Capability = t.GetRequiredCapabilityOrDefault();
                command.Value.AuthOnly = authOnly == true ? authOnly : t.GetIsAuthOnly();
                command.Value.Privileges = t.GetRequiredPrivilegesOrDefault();

                return command;
            }).ToList();

            var collection = new TypeInstance<CommandCollection>(col.Type, new CommandCollection(trigger,
                    messageType,
                    requiredPermissions,
                    requiredPrivileges,
                    authOnly,
                    childrenCommands,
                    new List<TypeInstance<CommandCollection>>()));

            var childrenCollections = col.Type.GetNestedTypes().Where(t => Attribute.IsDefined(t, typeof(CommandCollection))).Select(t => new TypeInstance<CommandCollection>(t, t.GetCustomAttribute<CommandCollection>())).ToList();

            foreach (var subCollection in childrenCollections)
                collection.Value.ChildrenCollections.Add(CreateCollection(subCollection));

            return collection;
        }

        private void ValidateCollection(TypeInstance<CommandCollection> collection)
        {
            var collectionMessageTypeAttrib = collection.Type.GetCustomAttribute<RequiredMessageType>();

            if (!Bot.UsingTranslations)
            {
                if (collection.Value.Trigger.Split(new char[] { '\n', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).Count() > 1)
                    throw new Exception("Triggers can only be 1 word long and contain no spaces, newlines or tabs");
            }
            else if (Bot.GetAllPhrasesByName(collection.Value.Trigger).Count == 0)
                throw new Exception($"Missing translation key {collection.Value.Trigger}\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");

            if (collection.Value.ChildrenCommands.Any(r => r.Type.GetParameters().Length > 0))
                throw new Exception($"Commands cannot contain generic parameters\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");

            if (collectionMessageTypeAttrib != null && collectionMessageTypeAttrib._messageType != MessageType.Both)
            {
                foreach (var command in collection.Value.ChildrenCommands)
                {
                    if (!Bot.UsingTranslations)
                    {
                        if (command.Value.Trigger.Split(new char[] { '\n', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).Count() > 1)
                            throw new Exception("Triggers can only be 1 word long and contain no spaces, newlines or tabs");
                    }
                    var commandMessageTypeAttrib = command.Type.GetCustomAttribute<RequiredMessageType>();

                    if (commandMessageTypeAttrib == null || commandMessageTypeAttrib._messageType == MessageType.Both)
                        continue;

                    if (collectionMessageTypeAttrib._messageType != commandMessageTypeAttrib._messageType)
                        throw new Exception($"Command {command.Value.Trigger} RequiredMessageType must be the same as the main collection RequiredMessageType");
                }
            }

            if (collection.Value.ChildrenCommands.Where(r => !string.IsNullOrWhiteSpace(r.Value.Trigger)).Any(command => collection.Value.ChildrenCollections.Any(s => s.Value.Trigger.IsEqual(command.Value.Trigger))))
                throw new Exception($"You have commands sharing the same trigger as subcollections in class {collection.Type.Name}\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");

            foreach (var subCollection in collection.Value.ChildrenCollections)
            {
                if (subCollection.Value.ChildrenCommands.Any(command => subCollection.Value.Trigger.IsEqual(command.Value.Trigger)))
                    throw new Exception($"Commands cannot have the same trigger as collection {collection.Value.Trigger}\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");

                if (collectionMessageTypeAttrib != null && collectionMessageTypeAttrib._messageType != MessageType.Both)
                {
                    var subCollectionMessageTypeAttrib = subCollection.Type.GetCustomAttribute<RequiredMessageType>();

                    if (subCollectionMessageTypeAttrib == null || subCollectionMessageTypeAttrib._messageType == MessageType.Both)
                        continue;

                    if (collectionMessageTypeAttrib._messageType != subCollectionMessageTypeAttrib._messageType)
                        throw new Exception($"Child Collection {subCollection.Value.Trigger} RequiredMessageType must be the same as the main collection RequiredMessageType");
                }

                ValidateCollection(subCollection);
            }
        }

        internal bool IsCommand(Message message)
        {
            var cmdArg = message.Content.Split(' ')[0];

            if (!Bot.UsingTranslations)
                return collections.Any(r => r.Value.Trigger.IsEqual(cmdArg)&& r.Value.MessageType == MessageType.Both ? true : r.Value.MessageType == message.MessageType);
           
            return collections.Any(r => Bot.GetAllPhrasesByName(r.Value.Trigger).Any(s => s.Value.IsEqual(message.Content.Split(' ')[0])) && (r.Value.MessageType == MessageType.Both ? true : r.Value.MessageType == message.MessageType));
        }
        private async Task<bool> ExecuteCommand(TypeInstance<CommandCollection> collection, MethodInstance<Command> command, Message message, CommandData commandData)
        {
            var colData = collection.Value;

            if (colData.MessageType != MessageType.Both && colData.MessageType != commandData.MessageType)
                return false;

            if (colData.AuthOnly && !Bot.IsAuthorized(commandData.SourceSubscriberId))
                return true;

            if (!await Bot.ValidatePermissions(commandData, colData.Capability, colData.Privileges))
                return true;

            var cmdData = command.Value;

            if (cmdData.MessageType != MessageType.Both && colData.MessageType != cmdData.MessageType)
                return false;

            if (cmdData.AuthOnly && !Bot.IsAuthorized(commandData.SourceSubscriberId))
                return true;

            if (!await Bot.ValidatePermissions(commandData, cmdData.Capability, cmdData.Privileges))
                return true;

            var i = (CommandContext)Activator.CreateInstance(collection.Type);

            i.Command = commandData;
            i.Bot = Bot;
            i.Message = message;
            try
            {
                command.Type.Invoke(i, null);

                return true;
            }
            catch (Exception d)
            {
                Bot.On.Emit(InternalEvent.INTERNAL_ERROR, $"Error executing command {command.Value.Trigger} please ensure that this method doesnt contain any parameters and try again");

                return true;
            }
        }

        private async Task<bool> ValidateAttributes(List<CustomAttribute> attributes, CommandData commandData)
        {
            foreach (var attrib in attributes)
                if (!await attrib.Validate(Bot, commandData))
                    return false;

            return true;
        }

        private async Task<bool> CheckCollection(TypeInstance<CommandCollection> collection, Message message, CommandData commandData, bool isSubCollection = false)
        {

            var cmdArg = commandData.Argument.Split(' ')[0];

            foreach (var subCollection in collection.Value.ChildrenCollections)
            {
                var phrase = Bot.UsingTranslations ? Bot.GetPhraseByName(commandData.Language, subCollection.Value.Trigger) : subCollection.Value.Trigger;

                if (phrase == null || !phrase.IsEqual(cmdArg) || !await ValidateAttributes(subCollection.CustomAttributes, commandData))
                    continue;

                commandData.Argument = string.Join(' ', commandData.Argument.Split(' ').Skip(1));

                if (await CheckCollection(subCollection, message, commandData, true))
                    return true;
            }

            foreach (var command in collection.Value.ChildrenCommands.Where(r => !string.IsNullOrWhiteSpace(r.Value.Trigger)).ToList())
            {
                var phrase = Bot.UsingTranslations ? Bot.GetPhraseByName(commandData.Language, command.Value.Trigger):command.Value.Trigger;

                if (phrase == null || !phrase.IsEqual(cmdArg) || !await ValidateAttributes(command.CustomAttributes, commandData))
                    continue;


                commandData.Argument = string.Join(' ', commandData.Argument.Split(' ').Skip(1));

                if (await ExecuteCommand(collection, command, message, commandData))
                    return true;
            }

            if (isSubCollection && collection.Value.Default != null && await ValidateAttributes(collection.Value.Default.CustomAttributes, commandData))
                return await ExecuteCommand(collection, collection.Value.Default, message, commandData);

            return false;
        }

        internal async Task ProcessMessage(Message message)
        {
            TypeInstance<CommandCollection> defaultCollection = null;
            MethodInstance<Command> defaultCommand = null;

            if (message.ContentType != ContentType.Text)
                return;

            var fixedArgs = string.Join(' ', message.Content.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries));

            var cmdArg = fixedArgs.Split(' ')[0];

            var commandData = new CommandData()
            {
                IsGroup = message.IsGroup,
                Argument = string.Join(' ', fixedArgs.Split(' ').Skip(1)),
                MessageType = message.MessageType
            };

            foreach (var collection in collections)
            {
                var phrase = Bot.GetAllPhrasesByName(collection.Value.Trigger).FirstOrDefault(r => r.Value.IsEqual(cmdArg));

                if (phrase != null || !Bot.UsingTranslations && collection.Value.Trigger.IsEqual(cmdArg))
                {
                    if (collection.Value.Default != null)
                    {
                        defaultCommand = collection.Value.Default;
                        defaultCollection = collection;
                    }

                    commandData.Subscriber = await Bot.GetSubscriberAsync(message.SourceSubscriberId);
                    commandData.Group = message.IsGroup ? await Bot.GetGroupAsync(message.SourceTargetId) : null;
                    commandData.Language = phrase == null ? "en" : phrase.Language;

                    if (await CheckCollection(collection, message, commandData))
                        return;
                }
                else
                    continue;
            }

            if (defaultCommand != null && await ValidateAttributes(defaultCommand.CustomAttributes, commandData))
                await ExecuteCommand(defaultCollection, defaultCommand, message, commandData);
        }

        internal void Load()
        {
            if (collections.Count > 0)
                return;

            var foundCollections = typeof(CommandContext).GetAllTypes().Where(t => Attribute.IsDefined(t, typeof(CommandCollection))).Select(t => new TypeInstance<CommandCollection>(t, t.GetCustomAttribute<CommandCollection>())).ToList();

            foreach (var col in foundCollections)
            {
                var collection = CreateCollection(col);

                ValidateCollection(collection);

                collections.Add(collection);
            }

            var duplicateCollections = collections.GroupBy(r => r.Value.Trigger.ToLower(), r => r.Value, (trigger, content) => new { trigger, commands = content.SelectMany(r => r.ChildrenCommands).ToList(), subCollections = content.SelectMany(r => r.ChildrenCollections).ToList() }).ToList();

            foreach (var duplicate in duplicateCollections)
            {
                if (duplicate.commands.Count(r => string.IsNullOrWhiteSpace(r.Value.Trigger)) > 1)
                    throw new Exception($"You can only have 1 default command in collection {duplicate.trigger}\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");

                if (duplicate.commands.GroupBy(r => r.Value.Trigger).Any(r => r.Count() > 1))
                    throw new Exception($"You can not have 2 of the same commands triggers in collection {duplicate.trigger}\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");

                if (duplicate.subCollections.GroupBy(r => r.Value.Trigger).Any(r => r.Count() > 1))
                    throw new Exception($"You can not have 2 of the same SubCommandCollections triggers in collection {duplicate.trigger}\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");
            }
        }

    }
}