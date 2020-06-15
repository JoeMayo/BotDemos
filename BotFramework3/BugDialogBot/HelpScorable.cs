using Microsoft.Bot.Connector;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables.Internals;

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
            var text = (activity as IMessageActivity)?.Text ?? "";
            var regex = new Regex("/help", RegexOptions.IgnoreCase);
            var match = regex.Match(text);

            return match.Success ? match.Groups[0].Value : null;
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