using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;

namespace PigLatinBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<Message> Post([FromBody]Message message)
        {
            string pigLatinReply = new PigLatin().FromEnglish(message.Text);

            return message.CreateReplyMessage(pigLatinReply);
        }
    }
}