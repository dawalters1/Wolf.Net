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

        private List<TypeInstance<Command>> Commands = new List<TypeInstance<Command>>();

        internal string GetCommandTriggerFromContent(string content)
        {
            foreach (var command in Commands)
            {
                var trigger = command.Value.Trigger;

                if (Bot.UsingTranslations)
                {
                    var phrase = Bot.GetAllPhrasesByName(trigger).OrderByDescending(r => r.Value.Length).FirstOrDefault(r => content.StartsWith(r.Value));

                    if (phrase != null)
                        return trigger;
                }
                else if (content.StartsWithCommand(trigger))
                    return trigger;
            }

            return null;
        }

        private async Task FindAndExecuteDefaultCommand(Message message, CommandData commandData)
        {
            var trigger = GetCommandTriggerFromContent(message.Content);

            if (string.IsNullOrWhiteSpace(trigger))
                return;

            var collections = Commands.Where(r => r.Value.Trigger.IsEqual(trigger)).ToList();

            if (collections.Count == 0)
                return;

            var foundCollection = collections.FirstOrDefault(r => r.Value.MethodInstances.Any(s => string.IsNullOrWhiteSpace(s.Value.Trigger)));

            if (foundCollection == null)
                return;

            if (!await ValidatePermissions(foundCollection, message, commandData) || !await ValidateAttributes(foundCollection, message, commandData))
                return;

            var command = foundCollection.Value.MethodInstances.FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Value.Trigger));

            if (!await ValidatePermissions(command, message, commandData)||!await ValidateAttributes(command, message, commandData))
                return;

            var phrase = Bot.GetAllPhrasesByName(trigger).OrderByDescending(r => r.Value.Length).FirstOrDefault(r => message.Content.StartsWith(r.Value.ToLower()));
            commandData.Argument = commandData.Argument[(phrase != null ? phrase.Value.Length : trigger.Length)..];
            commandData.Language = phrase != null ? phrase.Language : "en";

            ExecuteCommand(foundCollection, command, message, commandData);
        }

        private bool ExecuteCommand(TypeInstance<Command> collection, MethodInstance<Command> command, Message message, CommandData commandData)
        {
            var i = (CommandContext)Activator.CreateInstance(collection.Type);

            i.Command = commandData;
            i.Bot = Bot;
            i.Message = message;
            try
            {
                command.Type.Invoke(i, null);

                return true;
            }
            catch
            {
                Bot.On.Emit(InternalEvent.INTERNAL_ERROR, $"Error executing command {command.Value.Trigger} please ensure that this method doesnt contain any parameters and try again");

                return true;
            }
        }

        private async Task<bool> ValidateAttribute(RequiredPermissions requiredPermissions, Message message, CommandData commandData)
        {
            commandData.Group = message.IsGroup ? commandData.Group ?? await Bot.GetGroupAsync(message.SourceTargetId) : null;
            commandData.Subscriber ??= await Bot.GetSubscriberAsync(message.SourceSubscriberId);

            if (requiredPermissions == null)
                return true;

            return await requiredPermissions.Validate(Bot, commandData);
        }

        private async Task<bool> ValidatePermissions(MethodInstance<Command> methodInstance, Message message, CommandData commandData) => await ValidateAttribute(methodInstance.Type.GetCustomAttribute<RequiredPermissions>(), message, commandData);

        private async Task<bool> ValidatePermissions(TypeInstance<Command> typeInstance, Message message, CommandData commandData) => await ValidateAttribute(typeInstance.Type.GetCustomAttribute<RequiredPermissions>(), message, commandData);

        private async Task<bool> ValidateAttributes(List<CustomAttribute> customAttributes, Message message, CommandData commandData)
        {
            foreach (var attrib in customAttributes)
                if (!await attrib.Validate(Bot, commandData))
                    return false;

            return true;
        }
        private async Task<bool> ValidateAttributes(TypeInstance<Command> typeInstance, Message message, CommandData commandData)
        {
            commandData.Group = message.IsGroup ? commandData.Group ?? await Bot.GetGroupAsync(message.SourceTargetId) : null;
            commandData.Subscriber ??= await Bot.GetSubscriberAsync(message.SourceSubscriberId);

            if (await ValidatePermissions(typeInstance, message, commandData))
                return await ValidateAttributes(typeInstance.CustomAttributes, message, commandData);

            return false;
        }

        private async Task<bool> ValidateAttributes(MethodInstance<Command> methodInstance, Message message, CommandData commandData)
        {
            commandData.Group = message.IsGroup ? commandData.Group ?? await Bot.GetGroupAsync(message.SourceTargetId) : null;
            commandData.Subscriber ??= await Bot.GetSubscriberAsync(message.SourceSubscriberId);

            return await ValidateAttributes(methodInstance.CustomAttributes, message, commandData);
        }

        private MethodInstance<Command> ProcessCommands(List<MethodInstance<Command>> methodInstances, CommandData commandData, bool isSubCollection = false)
        {
            var content = commandData.Argument.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(content))
                return methodInstances.FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Value.Trigger));

            foreach (var command in methodInstances.Where(r => !string.IsNullOrWhiteSpace(r.Value.Trigger)).ToList())
            {
                var trigger = command.Value.Trigger;

                if (Bot.UsingTranslations)
                {
                    var phrase = Bot.GetAllPhrasesByName(trigger).OrderByDescending(r => r.Value.Length).FirstOrDefault(r => content.StartsWith(r.Value.ToLower()));

                    if (phrase != null)
                    {
                        if (!content.StartsWithCommand(phrase.Value))
                            continue;

                        commandData.Argument = content[phrase.Value.Length..].Trim();

                        commandData.Language ??= phrase.Language;
                    }
                    else
                        continue;
                }
                else
                {
                    if (!content.StartsWithCommand(trigger))
                        continue;

                    commandData.Argument = content[trigger.Length..].Trim();
                }


                return command;
            }

            return isSubCollection ? methodInstances.FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Value.Trigger)) : null;
        }

        private async Task<bool> ProcessCollectionAsync(TypeInstance<Command> typeInstance, Message message, CommandData commandData, bool isSubCollection = false)
        {
            var content = commandData.Argument.ToLower().Trim();

            var trigger = typeInstance.Value.Trigger;

            if (Bot.UsingTranslations)
            {
                var phrase = Bot.GetAllPhrasesByName(trigger).OrderByDescending(r => r.Value.Length).FirstOrDefault(r => content.StartsWith(r.Value.ToLower()));

                if (phrase != null)
                {
                    if (!content.StartsWithCommand(phrase.Value))
                        return false;

                    commandData.Argument = content[phrase.Value.Length..].Trim();
                    commandData.Language ??= phrase.Language;
                }
                else
                    return false;
            }
            else
            {
                if (!content.StartsWithCommand(trigger))
                    return false;

                commandData.Argument = content[trigger.Length..].Trim();
            }

            if (!await ValidatePermissions(typeInstance, message, commandData))
                return true;

            if (!await ValidateAttributes(typeInstance, message, commandData))
                return false;

            if (commandData.Subscriber.Privileges.HasFlag(Privilege.BOT) && Bot.IgnoreBots)
                return false;

            foreach (var subCollection in typeInstance.Value.TypeInstances)
            {
                var result = await ProcessCollectionAsync(subCollection, message, commandData, true);
                if (result)
                    return true;
            }

            var command = ProcessCommands(typeInstance.Value.MethodInstances, commandData, isSubCollection);

            if (command != null)
            {
                if (!await ValidatePermissions(typeInstance, message, commandData))
                    return true;

                if (!await ValidateAttributes(command, message, commandData))
                    return false;

                return ExecuteCommand(typeInstance, command, message, commandData);
            }


            return false;
        }

        internal async Task ProcessMessage(Message message)
        {

            foreach (var collection in Commands)
            {
                var result = ProcessCollectionAsync(collection, message, new CommandData(message));
                if (!await result)
                    continue;

                return;
            }

            await FindAndExecuteDefaultCommand(message, new CommandData(message));
        }

        private void ValidateCollection(TypeInstance<Command> typeInstance)
        {
            if (typeInstance.Value.MethodInstances.Any(r => r.Type.GetParameters().Length > 0))
                throw new Exception($"Commands cannot contain generic parameters\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");

            var parentMessageTypeAttrib = typeInstance.Type.GetCustomAttribute<RequiredMessageType>();

            var parentMessageType = parentMessageTypeAttrib != null ? parentMessageTypeAttrib.MessageType : MessageType.Both;

            if (parentMessageType != MessageType.Both)
            {
                foreach (var childMethodInstance in typeInstance.Value.MethodInstances)
                {
                    var childMessageTypeAttrib = childMethodInstance.Type.GetCustomAttribute<RequiredMessageType>();

                    var childMessageType = childMessageTypeAttrib != null ? childMessageTypeAttrib.MessageType : MessageType.Both;

                    if (childMessageType != MessageType.Both && childMessageType != parentMessageType)
                        throw new Exception("Commands must have the same message type as the parent collection");
                }
            }

            foreach(var childTypeInstance in typeInstance.Value.TypeInstances)
            {
                var childMessageTypeAttrib = childTypeInstance.Type.GetCustomAttribute<RequiredMessageType>();

                var childMessageType = childMessageTypeAttrib != null ? childMessageTypeAttrib.MessageType : MessageType.Both;

                if ((parentMessageType != MessageType.Both &&childMessageType!= MessageType.Both) && childMessageType != parentMessageType)
                    throw new Exception("Children collections must have the same message type as the parent collection");

                if(typeInstance.Value.MethodInstances.Any(r=>r.Value.Trigger.IsEqual(childTypeInstance.Value.Trigger)))
                    throw new Exception("You cannot have a command using the same trigger as a collection");

                ValidateCollection(childTypeInstance);
            }
        }

        private TypeInstance<Command> LoadCommandCollection(TypeInstance<Command> typeInstance)
        {
            var commands = typeInstance.Type.GetMethods().Where(method => Attribute.IsDefined(method, typeof(Command))).Select(t => new MethodInstance<Command>(t, t.GetCustomAttribute<Command>())).ToList();

            var collections = typeInstance.Type.GetNestedTypes().Where(t => Attribute.IsDefined(t, typeof(Command))).Select(t => new TypeInstance<Command>(t, t.GetCustomAttribute<Command>())).ToList();

            foreach (var subCollection in collections)
                typeInstance.Value.TypeInstances.Add(LoadCommandCollection(subCollection));

            typeInstance.Value.MethodInstances = commands;

            ValidateCollection(typeInstance);

            return typeInstance;
        }

        internal void Load(List<TypeInstance<Command>> typeInstances = null)
        {
            var commandCollections = typeInstances == null || typeInstances.Count == 0 ? typeof(CommandContext).GetAllTypes().Where(t => Attribute.IsDefined(t, typeof(Command))).Select(t => new TypeInstance<Command>(t, t.GetCustomAttribute<Command>())).ToList() : typeInstances;

            foreach (var collection in commandCollections)
                Commands.Add(LoadCommandCollection(collection));

            var duplicateCollections = Commands.GroupBy(r => r.Value.Trigger.ToLower(), r => r, (trigger, content) => new { trigger, containsRequiredPermissions = content.Select(r=> { return r.Type.GetCustomAttribute<RequiredPermissions>() != null; }).ToList(),  methodInstances = content.SelectMany(r => r.Value.MethodInstances).ToList(), typeInstances = content.SelectMany(r => r.Value.TypeInstances).ToList() }).ToList();

            foreach (var duplicate in duplicateCollections)
            {
                 if (duplicate.containsRequiredPermissions.Any(r=>r))
                     throw new Exception("You cannot have RequiredPermissions attribute in the main collection when several collections use the same trigger");
     
                if (duplicate.methodInstances.Count(r => string.IsNullOrWhiteSpace(r.Value.Trigger)) > 1)
                    throw new Exception($"You can only have 1 default command per collection key\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");

                if (duplicate.methodInstances.GroupBy(r => r.Value.Trigger).Any(r => r.Count() > 1))
                    throw new Exception($"You cannot have multiple commands using the same trigger per collection key\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");

                if (duplicate.typeInstances.GroupBy(r => r.Value.Trigger).Any(r => r.Count() > 1))
                    throw new Exception($"You cannot have multiple SubCollections using the same trigger per collection key\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");
            }
        }

        internal CommandManager(WolfBot bot)
        {
            Bot = bot;
        }
    }
}