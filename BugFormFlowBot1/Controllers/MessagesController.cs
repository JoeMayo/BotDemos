using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Net;

namespace BugFormFlowBot1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        internal static IDialog<BugReport> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(BugReport.BuildForm))
                        .Loop();
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity?.Type == ActivityTypes.Message)
                await Conversation.SendAsync(activity, MakeRootDialog);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}