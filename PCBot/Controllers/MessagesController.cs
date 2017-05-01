using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;

namespace PCBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            Activity reply = await new MessageHandler().ReactAsync(activity);

            if (reply != null)
                await connector.Conversations.ReplyToActivityAsync(reply);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}