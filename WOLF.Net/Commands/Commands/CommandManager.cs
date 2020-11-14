using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                _commands.Add(collection, collection.Type.GetMethods().Where(t => Attribute.IsDefined(t, typeof(Command))).Select(t => new MethodInstance<Command>(t, t.GetCustomAttribute<Command>())).ToList());
        }

        private KeyValuePair<TypeInstance<CommandCollection>, List<MethodInstance<Command>>> GetMatchingCollection(CommandData commandData)
        {
            Dictionary<TypeInstance<CommandCollection>, List<MethodInstance<Command>>> matches = new Dictionary<TypeInstance<CommandCollection>, List<MethodInstance<Command>>>();

            foreach (var collection in _commands)
            {
                var trigger = collection.Key.Value.Trigger;

                var phrases = Bot.GetAllPhrasesByName(trigger);

                if (phrases.Count > 0)
                {
                    var phrase = phrases.FirstOrDefault(r => Regex.IsMatch(r.Value, $@"{commandData.Argument}", RegexOptions.IgnoreCase));

                    if (phrase != null)
                    {
                        matches.Add(new TypeInstance<CommandCollection>(collection.Key.Type, collection.Key.Value.Clone(phrase.Value, phrase.Language)), collection.Value);
                        continue;
                    }
                }

                if (Regex.IsMatch(commandData.Argument, $@"\A{trigger}", RegexOptions.IgnoreCase))
                    matches.Add(collection.Key, collection.Value);

            }

            if (matches.Count == 0)
                return default;

            return matches.OrderByDescending(r => r.Key.Value.Trigger.Length).FirstOrDefault();

        }

        public MethodInstance<Command> GetMatchingCommand(List<MethodInstance<Command>> commands, CommandData data)
        {
            List<MethodInstance<Command>> matches = new List<MethodInstance<Command>>();

            foreach (var command in commands)
            {
                var trigger = command.Value.Trigger;

                if (data.IsTranslation)
                {
                    var phrase = Bot.GetPhraseByName(data.Language, trigger);

                    if (phrase != null)
                    {
                        matches.Add(new MethodInstance<Command>(command.Type, command.Value.Clone(phrase)));
                        continue;
                    }
                }

                if (Regex.IsMatch(data.Argument, $@"\A{trigger}", RegexOptions.IgnoreCase))
                    matches.Add(command);

            }

            if (matches.Count == 0)
                return null;

            return matches.OrderByDescending(r => r.Value.Trigger.Length).FirstOrDefault();

        }

        public async void ProcessMessage(Message message)
        {
            var commandData = new CommandData( message.SourceTargetId, message.SourceSubscriberId, message.Content, message.MessageType == Enums.Messages.MessageType.Group);

            //Lets get a matching collection based on data in the message
            var collection = GetMatchingCollection(commandData);

            //No collection matched the input
            if (collection.Key == null)
                return;

            //Collection trigger is valid, lets get or set the required information

            commandData.Language = collection.Key.Value.Language;

            commandData.Subscriber = await Bot.GetSubscriberAsync(message.SourceSubscriberId);

            commandData.Group = commandData.IsGroup ? await Bot.GetGroupAsync(message.SourceTargetId):null;

            //Check to see if all provided data passes the validation checks
            if (collection.Key.Attributes.Any(r => !r.Validate(Bot, commandData)))
                return;

            //Collection validation was successful, lets remove the command from the argument
            commandData.Argument = commandData.Argument.Remove(0, collection.Key.Value.Trigger.Length).Trim();

            //Lets get a matching command based on the remaining argument
            var command = GetMatchingCommand(collection.Value, commandData);

            //None Matched?
            if (command == null)
            {
                var defaultCommand = collection.Value.FirstOrDefault(r => r.Type.IsDefined(typeof(Default)));

                if (defaultCommand!=null)
                    DoCommand(collection.Key.Type, defaultCommand, commandData);

                return;
            }

            //Check to see if all the provided data passes the validation checks
            if (command.Attributes.Any(r => !r.Validate(Bot, commandData)))
                return;

            //Collection validation was successful, lets remove the command from the argument
            commandData.Argument = commandData.Argument.Remove(0, command.Value.Trigger.Length).Trim();

            DoCommand(collection.Key.Type, command, commandData);

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