using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Commands.Instances;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Enums.Subscribers;
using WOLF.Net.Utils;

namespace WOLF.Net.Commands.Form
{
    /// <summary>
    /// Manager for Form plugins
    /// </summary>
    public class FormManager
    {
        [JsonIgnore]
        private readonly WolfBot _bot;

        /// <summary>
        /// Called when a Form is canceled (for internal use)
        /// </summary>
        public event Action<IFormContext> FormCancelled = delegate { };
        /// <summary>
        /// Called when a Form is finished (for internal use)
        /// </summary>
        public event Action<IFormContext> FormFinished = delegate { };

        public event Action<IFormContext> FormTimeout = delegate { };

        /// <summary>
        /// Storage for all active group instances of the Form plugins
        /// </summary>
        private Dictionary<int, Dictionary<int, KeyValuePair<IFormContext, Action<string>>>> GroupInstances { get; set; } = new Dictionary<int, Dictionary<int, KeyValuePair<IFormContext, Action<string>>>>();

        /// <summary>
        /// Storage for all active private instances of the Form plugins
        /// </summary>
        private Dictionary<int, KeyValuePair<IFormContext, Action<string>>> PrivateInstances { get; set; } = new Dictionary<int, KeyValuePair<IFormContext, Action<string>>>();

        internal List<TypeInstance<Form>> Forms { get; set; } = new List<TypeInstance<Form>>();


        private async Task<bool> ValidateAttribute(RequiredPermissions requiredPermissions, Message message, FormData commandData)
        {
            commandData.Group = message.IsGroup ? commandData.Group ?? await _bot.GetGroupAsync(message.TargetGroupId) : null;
            commandData.Subscriber ??= await _bot.GetSubscriberAsync(message.SourceSubscriberId);

            if (requiredPermissions == null)
                return true;

            return await requiredPermissions.Validate(_bot, commandData.ToCommandData());
        }

        private async Task<bool> ValidatePermissions(TypeInstance<Form> typeInstance, Message message, FormData commandData) => await ValidateAttribute(typeInstance.Type.GetCustomAttribute<RequiredPermissions>(), message, commandData);

        private async Task<bool> ValidateAttributes(List<CustomAttribute> customAttributes, FormData commandData)
        {
            foreach (var attrib in customAttributes)
                if (!await attrib.Validate(_bot, commandData.ToCommandData()))
                    return false;

            return true;
        }
        private async Task<bool> ValidateAttributes(TypeInstance<Form> typeInstance, Message message, FormData commandData)
        {
            commandData.Group = message.IsGroup ? commandData.Group ?? await _bot.GetGroupAsync(message.TargetGroupId) : null;
            commandData.Subscriber ??= await _bot.GetSubscriberAsync(message.SourceSubscriberId);

            if (await ValidatePermissions(typeInstance, message, commandData))
                return await ValidateAttributes(typeInstance.CustomAttributes, commandData);

            return false;
        }

        /// <summary>
        /// See if subscriber has form in the current group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public bool HasGroupForm(int groupId, int subscriberId) => GroupInstances.ContainsKey(groupId) && GroupInstances[groupId].ContainsKey(subscriberId);

        /// <summary>
        /// Check to see if there are any forms for the current group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool GroupHasForms(int groupId) =>GroupInstances.ContainsKey(groupId) && GroupInstances[groupId].Count > 0;

        public bool GroupHasForms(int groupId, params int[] excludeIds) => GroupInstances.ContainsKey(groupId) && GroupInstances[groupId].Any(r=>excludeIds.Any(s=>s!=r.Key));

        /// <summary>
        /// Check to see if a subscriber has form in any of the groups
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public bool HasGroupForm(int subscriberId)=>GroupInstances.Any(t => t.Value.ContainsKey(subscriberId));

        /// <summary>
        /// Check to see if a subscriber has a private form
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public bool HasPrivateForm(int subscriberId) =>  PrivateInstances.ContainsKey(subscriberId);
        

        /// <summary>
        /// Cancel a subscribers form in a specific group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public bool CancelGroupForm(int groupId, int subscriberId)
        {
            if (GroupInstances.ContainsKey(groupId) && GroupInstances[groupId].ContainsKey(subscriberId))
            {
                GroupInstances[groupId][subscriberId].Key.Cancel();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Cancel all forms in a specific group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool CancelGroupForms(int groupId)
        {
            if (GroupInstances.ContainsKey(groupId) && GroupInstances[groupId].Count > 0)
            {
                GroupInstances[groupId].ToList().ForEach(r => r.Value.Key.Cancel());
                return true;
            }

            return false;
        }

        /// <summary>
        /// Cancel a subscribers form
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public bool CancelPrivateForm(int subscriberId)
        {
            if (PrivateInstances.ContainsKey(subscriberId))
            {
                PrivateInstances[subscriberId].Key.Cancel();
                return true;
            }

            return false;
        }

        internal async Task<bool> ProcessMessage(Message message)
        {
            try
            {

                if (message.IsGroup && GroupInstances.ContainsKey(message.TargetGroupId) && GroupInstances[message.TargetGroupId].ContainsKey(message.SourceSubscriberId))
                {
                    if (message.IsCommand)
                        return false;

                    var p = GroupInstances[message.TargetGroupId][message.SourceSubscriberId];
                    p.Key.Message = message;
                    p.Value(message.Content);
                    return true;
                }
                if (!message.IsGroup && PrivateInstances.ContainsKey(message.SourceSubscriberId))
                {
                    if (message.IsCommand)
                        return false;

                    var p = PrivateInstances[message.SourceSubscriberId];
                    p.Key.Message = message;
                    p.Value(message.Content);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _bot.On.Emit(Constants.Internal.ERROR, ex.ToString());
                return false;
            }

            return await FindForm(message);
        }

        private void AddForm(IFormContext form, Action<string> method, int groupId, int subscriberId, double duration)
        {
            if (!GroupInstances.ContainsKey(groupId))
                GroupInstances.Add(groupId, new Dictionary<int, KeyValuePair<IFormContext, Action<string>>>());
         
            GroupInstances[groupId].Add(subscriberId, new KeyValuePair<IFormContext, Action<string>>(form, method));
          
            form.TimeoutTimer = new System.Timers.Timer()
            {
                AutoReset = false,
                Enabled = true,
                Interval = duration
            };

            form.NextStage = (a) => GroupInstances[groupId][subscriberId] = new KeyValuePair<IFormContext, Action<string>>(form, a);
         
            form.Finish = () =>
            {
                form.TimeoutTimer.Stop();
                FormFinished(form);
                GroupInstances[groupId].Remove(subscriberId);
            };

            form.Cancel = () =>
            {
                form.TimeoutTimer.Stop();
                FormCancelled(form);
                GroupInstances[groupId].Remove(subscriberId);
            };

            form.ChangeTimeoutDelay = (i) =>
            {
                if (i <= 0)
                    return false;

                form.TimeoutTimer.Interval = i;
                return true;
            };

            form.TimeoutTimer.Elapsed += (a, d) =>
            {
                form.TimeoutTimer.Stop();
                FormTimeout(form);
                GroupInstances[groupId].Remove(subscriberId);
            };

            form.MoveToGroup = (i) => false;
            form.MoveToPrivate = () =>
            {
                if (PrivateInstances.ContainsKey(subscriberId))
                    return false;

                form.TimeoutTimer.Stop();
                AddForm(form, GroupInstances[groupId][subscriberId].Value, subscriberId,duration);
                GroupInstances[groupId].Remove(subscriberId);
                return true;
            };

            ((FormContext)form)._doStartUp(_bot);
        }

        private void AddForm(IFormContext form, Action<string> method, int subscriberId, double duration)
        {
            PrivateInstances.Add(subscriberId, new KeyValuePair<IFormContext, Action<string>>(form, method));

            form.TimeoutTimer = new System.Timers.Timer()
            {
                AutoReset = false,
                Enabled = true,
                Interval = duration
            };
            form.NextStage = (a) => PrivateInstances[subscriberId] = new KeyValuePair<IFormContext, Action<string>>(form, a);
            form.Finish = () =>
            {
                form.TimeoutTimer.Stop();
                FormFinished(form);
                PrivateInstances.Remove(subscriberId);
            };
            form.Cancel = () =>
            {
                form.TimeoutTimer.Stop();
                FormCancelled(form);
                PrivateInstances.Remove(subscriberId);
            };
            form.ChangeTimeoutDelay = (i) =>
            {
                if (i <= 0)
                    return false;

                form.TimeoutTimer.Interval = i;
                return true;
            };

            form.TimeoutTimer.Elapsed += (a, d) =>
            {
                FormTimeout(form);
                PrivateInstances.Remove(subscriberId);
            };

            form.MoveToPrivate = () => false;
            form.MoveToGroup = (i) =>
            {
                if (!GroupInstances.ContainsKey(i))
                    GroupInstances.Add(i, new Dictionary<int, KeyValuePair<IFormContext, Action<string>>>());
                if (GroupInstances[i].ContainsKey(subscriberId))
                    return false;

                form.TimeoutTimer.Stop();
                AddForm(form, PrivateInstances[subscriberId].Value, i, subscriberId);
                PrivateInstances.Remove(subscriberId);
                return true;
            };

            ((FormContext)form)._doStartUp(_bot);
        }

        private void ExecuteForm(TypeInstance<Form> form, Message message, FormData commandData, string args)
        {
            var i = (IFormContext)Activator.CreateInstance(form.Type);

            if (message.IsGroup)
                AddForm(i, i.Start, message.TargetGroupId, message.SourceSubscriberId, form.Value.Duration);
            else
                AddForm(i, i.Start, message.SourceSubscriberId,  form.Value.Duration);
            try
            {
                i.Bot = _bot;
                i.Message = message;
                i.Command = commandData;

                i.Start(args);
            }
            catch (Exception ex)
            {
                _bot.On.Emit(Constants.Internal.ERROR, ex.ToString());
            }
        }

        private async Task<bool> FindForm(Message message)
        {

            if (_bot.Configuration.UseTranslations)
            {
                var forms = Forms.Where(r => _bot.Phrases.Any(s => s.Name.IsEqual(r.Value.Trigger))).ToList();

                if (forms.Count > 0)
                {
                    var phrase = _bot.Phrases.Where(r => forms.Any(s => s.Value.Trigger.IsEqual(r.Name)) && message.Content.StartsWithCommand(r.Value)).OrderByDescending(r => r.Value.Length).FirstOrDefault();

                    if (phrase == null)
                        return false;

                    var form = Forms.FirstOrDefault(r=>r.Value.Trigger==phrase.Name);

                    if (form == null)
                        return false;

                    var commandData = new FormData(message)
                    {
                        Group = message.IsGroup ? await _bot.GetGroupAsync(message.TargetGroupId) : null,
                        Subscriber = await _bot.GetSubscriberAsync(message.SourceSubscriberId),
                         Language = phrase.Language
                    };

                    if (!await ValidatePermissions(form, message, commandData))
                        return true;

                    if (!await ValidateAttributes(form, message, commandData))
                        return false;

                    if (commandData.Subscriber.Privileges.HasFlag(Privilege.BOT) && _bot.Configuration.IgnoreOfficialBots)
                        return true;

                    ExecuteForm(form, message, commandData, message.Content.Remove(0, phrase.Value.Length).Trim());

                    return true;
                }
                return false;
            }
            else
            {
                var forms = Forms.Where(r => message.Content.StartsWithCommand(r.Value.Trigger)).OrderByDescending(r=>r.Value.Trigger.Length).ToList();

                if (forms.Count > 0)
                {
                    var form = Forms.FirstOrDefault();

                    if (form == null)
                        return false;

                    var commandData = new FormData(message)
                    {
                        Group = message.IsGroup ? await _bot.GetGroupAsync(message.TargetGroupId) : null,
                        Subscriber = await _bot.GetSubscriberAsync(message.SourceSubscriberId)
                    };

                    if (!await ValidatePermissions(form, message, commandData))
                        return true;

                    if (!await ValidateAttributes(form, message, commandData))
                        return false;

                    if (commandData.Subscriber.Privileges.HasFlag(Privilege.BOT) && _bot.Configuration.IgnoreOfficialBots)
                        return true;

                    ExecuteForm(form, message, commandData, message.Content.Remove(0, form.Value.Trigger.Length).Trim());

                    return true;
                }
                return false;
            }
        }

        internal void Load()
        {
            if (Forms.Count > 0)
                return;

            Forms = typeof(FormContext).GetAllTypes().Where(t => Attribute.IsDefined(t, typeof(Form))).Select(t => new TypeInstance<Form>(t, t.GetCustomAttribute<Form>())).ToList();

            if (Forms.GroupBy(r => r.Value.Trigger.ToLower()).Any(r => r.Count() > 1))
                throw new Exception($"You cannot have multiple forms using the same trigger\nPlease take a look at the new V4 command layout: https://github.com/dawalters1/Wolf.Net");
        }

        internal FormManager(WolfBot bot)
        {    
            _bot = bot;
        }
    }
}
