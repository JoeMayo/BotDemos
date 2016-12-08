using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Net;

namespace BugDialogBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity?.Type == ActivityTypes.Message)
                await Conversation.SendAsync(activity, () => new BugReportDialog());

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}