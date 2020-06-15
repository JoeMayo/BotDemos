using BugDialogBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BugDialogBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        readonly ILogger logger;

        List<string> reportWords = new List<string> { "NEW REPORT", "NEW", "REPORT", "BUG" };

        public MainDialog(ReportDialog reportDialog, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            this.logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(reportDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var messageText = $"You can type 'New Report' to submit a new bug report.";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string userResponse = (stepContext.Result as string)?.ToUpper() ?? string.Empty;
            if (reportWords.Contains(userResponse))
            {
                return await stepContext.BeginDialogAsync(nameof(ReportDialog), null, cancellationToken);
            }
            else
            {
                string unrecognizedResponse =
                    "I can't do anything other than bug reports. " +
                    "Just type 'New Report' to start a new bug report.";
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(unrecognizedResponse), cancellationToken);
                return await stepContext.ReplaceDialogAsync(InitialDialogId, null, cancellationToken);
            }
        }

        async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is BugReport rpt)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text(
                        $"Your bug report with the following values has been submitted:\n" +
                        $"\n" +
                        $"* **Platform:** {rpt.Platform}\n" +
                        $"* **Product:**  {rpt.Product}\n" +
                        $"* **Description:** {rpt.Description}\n" +
                        $"\n" +
                        $"Thanks for the feedback!"));
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Not submitted - you're welcome to submit another bug report any time."));
            }

            return await stepContext.ReplaceDialogAsync(InitialDialogId, null, cancellationToken);
        }
    }
}
