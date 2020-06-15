using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace PigLatinBot
{
    public class MainBot : ActivityHandler
    {
        readonly IPigLatin pigLatin;

        public MainBot(IPigLatin pigLatin)
        {
            this.pigLatin = pigLatin;
        }

        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    string welcomeText = 
                        $"Hi, I'm Pig Latin Bot. I translate everything " +
                        $"you say to me into Pig Latin. Check out " +
                        $"[How to Speak Pig Latin](https://www.wikihow.com/Speak-Pig-Latin) " +
                        $"for more info.";
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText), cancellationToken);
                }
            }
        }

        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string translation = pigLatin.FromEnglish(turnContext.Activity.Text);
            await turnContext.SendActivityAsync(MessageFactory.Text(translation), cancellationToken);
        }
    }
}
