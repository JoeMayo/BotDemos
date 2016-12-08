using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace ContactInfoBot
{
    [LuisModel("", "")]
    [Serializable]
    public class ContactInfoDialog : LuisDialog<object>
    {
        public const string ContactType = "ContactType";
        string currentEntity = "";

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string userUtterance = result.Query;
            await context.PostAsync($"Sorry, I didn't understand \"{userUtterance}\".");
            context.Wait(MessageReceived);
        }

        [LuisIntent("ChangeInfo")]
        public async Task ChangeInfo(IDialogContext context, LuisResult result)
        {
            EntityRecommendation entityRec;
            result.TryFindEntity(ContactType, out entityRec);

            currentEntity = entityRec.Entity;

            PromptDialog.Text(
                context: context,
                resume: ResumeAndHandleTextAsync,
                prompt: $"What would you like to change your {currentEntity} to?",
                retry: "I didn't understand. Please try again.");
        }

        public async Task ResumeAndHandleTextAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string newEntityValue = await argument;

            await context.PostAsync($"Your {currentEntity} is now {newEntityValue}");

            context.Wait(MessageReceived);
        }
    }
}
