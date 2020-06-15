using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Net;

namespace PigLatinBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity?.Type == ActivityTypes.Message)
            {
                string pigLatinReply = new PigLatin().FromEnglish(activity.Text);

                Activity reply = activity.CreateReply(pigLatinReply);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                await HandleSystemMessageAsync(activity);
            }


            return Request.CreateResponse(HttpStatusCode.OK);
        }

        async Task HandleSystemMessageAsync(Activity activity)
        {
            const string WelcomeMessage = @"
# Welcome to Pig Latin Bot!
Type anything and I'll convert it to Pig Latin.";

            if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                Func<ChannelAccount, bool> isChatbot =
                            channelAcct => channelAcct.Id == activity.Recipient.Id;

                if (activity.MembersAdded?.Any(isChatbot) ?? false)
                {
                    Activity reply = (activity as Activity).CreateReply(WelcomeMessage);

                    var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
        }
    }
}