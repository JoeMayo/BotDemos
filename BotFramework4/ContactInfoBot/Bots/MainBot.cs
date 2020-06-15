// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace ContactInfoBot.Bots
{
    public class MainBot<T> : ActivityHandler
        where T : Dialog
    {
        readonly ConversationState conversationState;
        readonly UserState userState;
        readonly T dialog;
        ILogger<MainBot<T>> logger;

        public MainBot(ConversationState conversationState, UserState userState, T dialog, ILogger<MainBot<T>> logger)
        {
            this.dialog = dialog;
            this.conversationState = conversationState;
            this.userState = userState;
            this.logger = logger;
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await dialog.RunAsync(turnContext, conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    string welcomeText =
                        $"Hi, I'm Bug Dialog Bot. " +
                        $"I simulate a bot that can help write bug reports.";
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText), cancellationToken);
                    await dialog.RunAsync(turnContext, conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
                }
            }
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn.
            await conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
    }
}
