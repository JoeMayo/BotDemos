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

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}