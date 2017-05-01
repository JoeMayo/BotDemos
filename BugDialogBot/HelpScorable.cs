using Microsoft.Bot.Connector;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Scorables;

namespace BugDialogBot
{
    public class HelpScorable : ScorableBase<IActivity, string, double>
    {
        readonly IBotToUser botToUser;

        public HelpScorable(IBotToUser botToUser)
        {
            this.botToUser = botToUser;
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var regex = new Regex("help", RegexOptions.IgnoreCase);
            var message = activity as IMessageActivity;
            if (message != null && message.Text != null)
            {
                var text = message.Text;
                var match = regex.Match(text);
                if (match.Success)
                {
                    return match.Groups[0].Value;
                }
            }

            return null;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            await botToUser.PostAsync("Help is on the way!", cancellationToken: token);
        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}