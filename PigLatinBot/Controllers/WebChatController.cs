using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PigLatinBot
{
    public class WebChatController : ApiController
    {
        public async Task<string> Get()
        {
            string webChatSecret = ConfigurationManager.AppSettings["WebChatSecret"];

            // This technique is insecure because you're exposing your secret in the Web page
            return $"<iframe width='400px' height='400px' src='https://webchat.botframework.com/embed/PigLatinBotJoeMayo?s={webChatSecret}'></iframe>";

            //var request = new HttpRequestMessage(HttpMethod.Get, "https://webchat.botframework.com/api/tokens");
            //request.Headers.Add("Authorization", "BOTCONNECTOR " + webChatSecret);

            //HttpResponseMessage response = await new HttpClient().SendAsync(request);
            //string token = await response.Content.ReadAsStringAsync();
            //token = token.Replace("\"", "");

            //return $"<iframe width='400px' height='400px' src='https://webchat.botframework.com/embed/PigLatinBotJoeMayo?t={token}'></iframe>";
        }
    }
}