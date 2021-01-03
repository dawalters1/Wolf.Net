using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using WOLF.Net.Entities.Messages;

namespace WOLF.Net.Commands.Form
{
    public interface IFormContext
    {
        WolfBot Bot { get; set; }
        Message Message { get; set; }

        FormData Command { get; set; }

        void Start(string message);

        /// <summary>
        /// The next action in line
        /// </summary>
        Action<Action<string>> NextStage { get; set; }

        /// <summary>
        /// What to do when finished.
        /// </summary>
        Action Finish { get; set; }

        Action Cancel { get; set; }
        /// <summary>
        /// Move the questionnaire to a private conversation (can fail if already in private conversation or the user already has a private questionnaire going).
        /// </summary>
        Func<bool> MoveToPrivate { get; set; }

        /// <summary>
        /// Move the questionnaire to a group conversation (can fail if already in a group conversation or the user already has a group questionnaire going).
        /// </summary>
        Func<int, bool> MoveToGroup { get; set; }

    /// <summary>
        /// Event that gets called upon the questionnaire being canceled.
        /// </summary>
        event Action OnCancel;

        /// <summary>
        /// Event that gets called upon the questionnaire being finished.
        /// </summary>
        event Action OnFinish;

        event Action OnTimeout;

        Func<int, bool> ChangeTimeoutDelay { get; set; }

        internal Timer TimeoutTimer { get; set; }
    }
}
