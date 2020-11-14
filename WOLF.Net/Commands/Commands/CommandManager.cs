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
using WOLF.Net.Entities.Messages;
using WOLF.Net.Utilities;

namespace WOLF.Net.Commands.Commands
{
    public class CommandManager
    {
        private WolfBot Bot;

        //House the CommandCollection and its Children
        private Dictionary<TypeInstance<CommandCollection>, List<MethodInstance<Command>>> _commands = new Dictionary<TypeInstance<CommandCollection>, List<MethodInstance<Command>>>();

        public CommandManager(WolfBot bot)
        {
            Bot = bot;

            var collections = typeof(CommandContext).GetAllTypes().Where(t => Attribute.IsDefined(t, typeof(CommandCollection))).Select(t => new TypeInstance<CommandCollection>(t, t.GetCustomAttribute<CommandCollection>()));

            foreach (var collection in collections)
            {
                if (collection.Type.GetCustomAttribute<AuthOnly>() != null && collection.Type.GetCustomAttribute<RequiredPermissions>() != null)
                    throw new Exception("You can only have either AuthOnly or RequiredPermissions in CommandCollection not both");

                var commands = collection.Type.GetMethods().Where(t => Attribute.IsDefined(t, typeof(Command))).Select(t => new MethodInstance<Command>(t, t.GetCustomAttribute<Command>())).ToList();
             
                if (commands.Any(r=>r.Type.GetCustomAttribute<AuthOnly>() != null && r.Type.GetCustomAttribute<RequiredPermissions>() != null))
                    throw new Exception("You can only have either AuthOnly or RequiredPermissions in a Command not both");

                _commands.Add(collection, commands);              
            }
        }

        public async Task ProcessMessage(Message message)
        {
            var commandData = new CommandData(message.SourceTargetId, message.SourceSubscriberId, message.Content, message.MessageType == Enums.Messages.MessageType.Group);

            string language = null;

            Dictionary<TypeInstance<CommandCollection>, List<MethodInstance<Command>>> matching = new Dictionary<TypeInstance<CommandCollection>, List<MethodInstance<Command>>>();

            foreach (var collection in _commands)
            {
                var cmdData = commandData.Clone();

                var collectionTrigger = collection.Key.Value.Trigger;

                var collectionPhrase = Bot.GetAllPhrasesByName(collectionTrigger).Where(r => Regex.IsMatch(r.Value, $@"{cmdData.Argument}", RegexOptions.IgnoreCase)).FirstOrDefault();

                if (collectionPhrase != null)
                {
                    matching.Add(new TypeInstance<CommandCollection>(collection.Key.Type, collection.Key.Value.Clone(collectionPhrase.Value, collectionPhrase.Language)), new List<MethodInstance<Command>>());
                    language = collectionPhrase.Language;
                    cmdData.Argument = cmdData.Argument.Remove(0, collectionPhrase.Value.Length).Trim();
                }
                else if (Regex.IsMatch(cmdData.Argument, $@"\A{collectionTrigger}", RegexOptions.IgnoreCase))
                {
                    matching.Add(collection.Key, new List<MethodInstance<Command>>());
                    cmdData.Argument = cmdData.Argument.Remove(0, collectionTrigger.Length).Trim();
                }
                else
                    continue;

                foreach (var command in collection.Value.ToList())
                {
                    if (language != null)
                    {
                        var commandPhrase = Bot.GetAllPhrasesByLanguageAndName(language, command.Value.Trigger).Where(r => Regex.IsMatch(r.Value, $@"{cmdData.Argument}", RegexOptions.IgnoreCase)).FirstOrDefault();

                        if (commandPhrase != null)
                            matching[collection.Key].Add(new MethodInstance<Command>(command.Type, command.Value.Clone(commandPhrase.Value)));
                    }
                    else if (Regex.IsMatch(cmdData.Argument, $@"\A{command.Value.Trigger}", RegexOptions.IgnoreCase))
                        matching[collection.Key].Add(command);
                }
            }

            if (matching.Count == 0)
                return;

            commandData.Subscriber = await Bot.GetSubscriberAsync(commandData.SourceSubscriberId);

            commandData.Group = commandData.IsGroup ? await Bot.GetGroupAsync(commandData.SourceTargetId) : null;

            var sorted = matching.OrderByDescending(r => r.Key.Value.Trigger.Length).ToList();

            sorted.RemoveAll(r => r.Key.Value.Trigger.Length != sorted[0].Key.Value.Trigger.Length);

            var commandToCall = sorted.SelectMany(r => r.Value).OrderByDescending(r => r.Value.Trigger.Length).FirstOrDefault();

            var collectionToCall = sorted.Where(r => r.Value.Any(s => s.Value.Trigger == commandToCall.Value.Trigger)).FirstOrDefault();

            foreach (var attrib in collectionToCall.Key.Attributes)
                if (!await attrib.Validate(Bot, commandData))
                    return;

            commandData.Language = collectionToCall.Key.Value.Language;

            commandData.Argument = commandData.Argument.Remove(0, collectionToCall.Key.Value.Trigger.Length).Trim();

            if (commandToCall == null)
            {
                var defaultCommand = collectionToCall.Value.FirstOrDefault(r => r.Type.IsDefined(typeof(Default)));

                if (defaultCommand != null)
                    DoCommand(collectionToCall.Key.Type, defaultCommand, commandData);

                return;
            }

            foreach (var attrib in commandToCall.Attributes)
                if (!await attrib.Validate(Bot, commandData))
                    return;

            commandData.Argument = commandData.Argument.Remove(0, commandToCall.Value.Trigger.Length).Trim();

            DoCommand(collectionToCall.Key.Type, commandToCall, commandData);
        }

        private void DoCommand(Type type, MethodInstance<Command> command, CommandData commandData)
        {
            var i = (CommandContext)Activator.CreateInstance(type);

            i.Command = commandData;
            i.Bot = Bot;

            command.Type.Invoke(i, null);
        }
    }
}