using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;

namespace PCBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<Message> Post([FromBody]Message message)
        {
            return new MessageHandler().React(message);
        }
    }
}