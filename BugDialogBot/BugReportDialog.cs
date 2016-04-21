using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BugDialogBot
{
    [Serializable]
    class BugReportDialog : IDialog<object>
    {
        ProductOptions productOptions;
        PlatformOptions platformOptions;
        string description;
        
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(ConversationStartedAsync);
        }

        public async Task ConversationStartedAsync(IDialogContext context, IAwaitable<Message> argument)
        {
            Message message = await argument;
            await context.PostAsync(message.Text);

            PromptDialog.Choice(
                context: context,
                resume: ResumeAndPromptPlatformAsync,
                options: Enum.GetValues(typeof(ProductOptions)).Cast<ProductOptions>().ToArray(),
                prompt: "Welcome to the Bug Report Bot! Please select the product you're having a problem with (SQLServer or VisualStudio):",
                retry: "I didn't understand. Please try again.");
        }

        public async Task ResumeAndPromptPlatformAsync(IDialogContext context, IAwaitable<ProductOptions> argument)
        {
            productOptions = await argument;

            PromptDialog.Choice(
                context: context,
                resume: ResumeAndPromptDescriptionAsync,
                options: Enum.GetValues(typeof(PlatformOptions)).Cast<PlatformOptions>().ToArray(),
                prompt: "Which platform did the problem occur on? (Linux, Mac, or Windows):",
                retry: "I didn't understand. Please try again.");
        }

        public async Task ResumeAndPromptDescriptionAsync(IDialogContext context, IAwaitable<PlatformOptions> argument)
        {
            platformOptions = await argument;

            PromptDialog.Text(
                context: context,
                resume: ResumeAndPromptSummaryAsync,
                prompt: "Please provide a detailed description of the problem:",
                retry: "I didn't understand. Please try again.");
        }

        public async Task ResumeAndPromptSummaryAsync(IDialogContext context, IAwaitable<string> argument)
        {
            description = await argument;

            PromptDialog.Confirm(
                context: context,
                resume: ResumeAndHandleConfirmAsync,
                prompt: $"You entered '{productOptions}', '{platformOptions}', and '{description}'. Is that correct?",
                retry: "I didn't understand. Please try again.");
        }

        public async Task ResumeAndHandleConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            bool choicesAreCorrect = await argument;

            if (choicesAreCorrect)
                await context.PostAsync("Your bug report has been submitted. Thanks for the feedback!");
            else
                await context.PostAsync("I see. You're welcome to try again.");

            context.Wait(ConversationStartedAsync);
        }
    }
}
