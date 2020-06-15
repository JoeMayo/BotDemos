using BugDialogBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BugDialogBot.Dialogs
{
    public class ReportDialog : ComponentDialog
    {
        const string ProductID = "ProductID";
        const string PlatformID = "PlatformID";
        const string DescriptionID = "DescriptionID";

        protected readonly ILogger Logger;

        public ReportDialog(ILogger<MainDialog> logger)
            : base(nameof(ReportDialog))
        {
            Logger = logger;

            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ProductStepAsync,
                PlatformStepAsync,
                DescriptionStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        async Task<DialogTurnResult> ProductStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // You can send a message back to a user at any time.
            string welcomeText = $"Now preparing a new bug report - I just have a few questions";
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText));

            IEnumerable<string> options = Enum.GetNames(typeof(ProductOptions)).Cast<string>();
            string requestText = $"Please select the product you're having a problem with ({string.Join(", ", options)})";
            string retryText = $"That option isn't available! {requestText}:";

            Activity promptMessage = MessageFactory.Text(requestText, requestText, InputHints.ExpectingInput);
            Activity retryMessage = MessageFactory.Text(retryText, retryText, InputHints.ExpectingInput);

            var choices =
                (from product in options
                 select new Choice
                 {
                     Value = product.ToString()
                 })
                .ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = promptMessage,
                Choices = choices,
                RetryPrompt = retryMessage
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        async Task<DialogTurnResult> PlatformStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[ProductID] = (stepContext.Result as FoundChoice)?.Value;

            IEnumerable<string> options = Enum.GetNames(typeof(PlatformOptions)).Cast<string>();
            string requestText = $"Which platform did the problem occur on? ({string.Join(", ", options)})";
            string retryText = $"That option isn't available! {requestText}:";

            Activity promptMessage = MessageFactory.Text(requestText, requestText, InputHints.ExpectingInput);
            Activity retryMessage = MessageFactory.Text(retryText, retryText, InputHints.ExpectingInput);

            var choices =
                (from product in options
                 select new Choice
                 {
                     Value = product.ToString()
                 })
                .ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = promptMessage,
                Choices = choices,
                RetryPrompt = retryMessage
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        async Task<DialogTurnResult> DescriptionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[PlatformID] = (stepContext.Result as FoundChoice)?.Value;

            string requestText = $"Please provide a detailed description of the problem:";
            string retryText = $"I didn't understand! {requestText}:";

            Activity promptMessage = MessageFactory.Text(requestText, requestText, InputHints.ExpectingInput);
            Activity retryMessage = MessageFactory.Text(retryText, retryText, InputHints.ExpectingInput);

            var promptOptions = new PromptOptions
            {
                Prompt = promptMessage,
                RetryPrompt = retryMessage
            };

            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[DescriptionID] = stepContext.Result.ToString();

            string product = stepContext.Values[ProductID].ToString();
            string platform = stepContext.Values[PlatformID].ToString();
            string description = stepContext.Values[DescriptionID].ToString();

            string requestText = $"You entered '{product}', '{platform}', and '{description}'. Is that correct?";
            string retryText = $"I didn't understand! {requestText}:";

            Activity promptMessage = MessageFactory.Text(requestText, requestText, InputHints.ExpectingInput);
            Activity retryMessage = MessageFactory.Text(retryText, retryText, InputHints.ExpectingInput);

            var promptOptions = new PromptOptions
            {
                Prompt = promptMessage,
                RetryPrompt = retryMessage
            };

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), promptOptions, cancellationToken);
        }

        async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            BugReport bugRpt = null;

            bool choicesAreCorrect = (bool)stepContext.Result;
            if (choicesAreCorrect)
            {
                IDictionary<string, object> vals = stepContext.Values;

                Enum.TryParse(vals[PlatformID] as string, out PlatformOptions platform);
                Enum.TryParse(vals[ProductID] as string, out ProductOptions product);
                string description = vals[DescriptionID] as string;

                bugRpt = new BugReport
                {
                    Platform = platform,
                    Product = product,
                    Description = description
                };
            }

            return await stepContext.EndDialogAsync(bugRpt, cancellationToken);
        }
    }
}
