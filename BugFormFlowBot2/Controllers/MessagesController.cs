using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System.Net.Http;
using System.Net;

namespace BugFormFlowBot2
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        internal static IDialog<BugReport> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(BugReport.BuildForm))
                        .Loop();
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity?.Type == ActivityTypes.Message)
                await Conversation.SendAsync(activity, MakeRootDialog);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}