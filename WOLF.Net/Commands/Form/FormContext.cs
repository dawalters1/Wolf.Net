using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;

namespace WOLF.Net.Commands.Form
{
    public abstract class FormContext : IFormContext
    {
        public WolfBot Bot { get; set; }

        public FormData Command { get; set; }

        public Message Message { get; set; }

        public async Task<Response<MessageResponse>> ReplyAsync(object content, bool includeEmbeds = false) => await SendMessageAsync(content, includeEmbeds);

        public async Task<Response<MessageResponse>> SendMessageAsync(object content, bool includeEmbeds = false) => await Bot.Messaging().SendMessageAsync(Command.IsGroup ? Command.TargetGroupId : Command.SourceSubscriberId, content, Command.MessageType, includeEmbeds);

        public async Task<Response<MessageResponse>> SendPrivateMessageAsync(int subscriberId, object content, bool includeEmbeds = false) => await Bot.Messaging().SendPrivateMessageAsync(subscriberId, content, includeEmbeds);

        public async Task<Response<MessageResponse>> SendGroupMessageAsync(int targetGroupId, object content, bool includeEmbeds = false) => await Bot.Messaging().SendGroupMessageAsync(targetGroupId, content, includeEmbeds);

        [JsonIgnore]
        public Action Finish { get; set; }

        [JsonIgnore]
        public Func<int, bool> MoveToGroup { get; set; }

        [JsonIgnore]
        public Func<bool> MoveToPrivate { get; set; }

        [JsonIgnore]
        public Action<Action<string>> NextStage { get; set; }

        [JsonIgnore]
        public Func<int, bool> ChangeTimeoutDelay { get; set; }

        [JsonIgnore]
        public Action Cancel { get; set; }

        public event Action OnCancel = delegate { };

        public event Action OnFinish = delegate { };

        public event Action OnTimeout = delegate { };


        private bool DoneStartUp = false;
        /// <summary>
        /// Does the strart up handling for Questionnaires for the QuestionnaireCanceled and QuestionniareFinished delegates
        /// </summary>
        /// <param name="bot"></param>
        internal void _doStartUp(WolfBot bot)
        {
            if (!DoneStartUp)
            {
                bot.FormManager.FormCancelled += (i) =>
                {
                    if (i == this)
                        OnCancel();
                };

                bot.FormManager.FormFinished += (i) =>
                {
                    if (i == this)
                        OnFinish();
                };

                bot.FormManager.FormTimeout += (i) =>
                {
                    if (i == this)
                        OnTimeout();
                };

                DoneStartUp = true;
            }

        }
        public abstract void Start(string message);

        [JsonIgnore]
        Timer IFormContext.TimeoutTimer { get; set; }
    }
}