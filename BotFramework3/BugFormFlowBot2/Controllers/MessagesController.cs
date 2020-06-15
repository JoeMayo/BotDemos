using System;
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
                try
                {
                    await Conversation.SendAsync(activity, MakeRootDialog);
                }
                catch (FormCanceledException fcEx) when(fcEx.InnerException is TooManyAttemptsException)
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                    Activity reply = activity.CreateReply(
                        $"Too Many Attempts at {fcEx.Last}. " +
                        $"Completed Steps: {string.Join(", ", fcEx.Completed)}");

                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                catch (FormCanceledException fcEx)
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                    Activity reply = activity.CreateReply(
                        $"Form cancelled at {fcEx.Last}. " +
                        $"Completed Steps: {string.Join(", ", fcEx.Completed)}");

                    await connector.Conversations.ReplyToActivityAsync(reply);
                }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}