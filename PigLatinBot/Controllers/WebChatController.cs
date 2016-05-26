using Newtonsoft.Json;
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

            return await GetIFrameWithTokenAsync(webChatSecret);

            //return await GetIFrameViaPostWithToken(webChatSecret);
        }

        string GetIFrameWithSecret(string webChatSecret)
        {
            // This technique is insecure because you're exposing your secret in the Web page
            return $"<iframe width='400px' height='400px' src='https://webchat.botframework.com/embed/PigLatinBotJoeMayo?s={webChatSecret}'></iframe>";
        }

        async Task<string> GetIFrameWithTokenAsync(string webChatSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://webchat.botframework.com/api/tokens");
            request.Headers.Add("Authorization", "BOTCONNECTOR " + webChatSecret);

            HttpResponseMessage response = await new HttpClient().SendAsync(request);
            string token = await response.Content.ReadAsStringAsync();
            token = token.Replace("\"", "");

            return $"<iframe width='400px' height='400px' src='https://webchat.botframework.com/embed/PigLatinBotJoeMayo?t={token}'></iframe>";
        }

        //async Task<string> GetIFrameViaPostWithToken(string webChatSecret)
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://webchat.botframework.com/api/conversations");
        //    request.Headers.Add("Authorization", "BOTCONNECTOR " + webChatSecret);

        //    HttpResponseMessage response = await new HttpClient().SendAsync(request);
        //    string responseJson = await response.Content.ReadAsStringAsync();
        //    WebChatTokenResponse webChatResponse = JsonConvert.DeserializeObject<WebChatTokenResponse>(responseJson);

        //    return $"<iframe width='400px' height='400px' src='https://webchat.botframework.com/embed/PigLatinBotJoeMayo?t={webChatResponse.Token}'></iframe>";
        //}
    }
}