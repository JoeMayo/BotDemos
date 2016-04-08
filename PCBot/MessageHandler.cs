using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBot
{
    public class MessageHandler
    {
        public Message React(Message eventMessage)
        {
            Message responseMessage = null;

            BotEvent eventType = GetBotEvent(eventMessage.Type);

            switch (eventType)
            {
                case BotEvent.BotAddedToConversation:
                    responseMessage = ReactToBotAdded(eventMessage);
                    break;
                case BotEvent.BotRemovedFromConversation:
                    responseMessage = ReactToBotRemoved(eventMessage);
                    break;
                case BotEvent.DeleteUserData:
                    responseMessage = ReactToDeleteUser(eventMessage);
                    break;
                case BotEvent.EndOfConversation:
                    responseMessage = ReactToEndConversation(eventMessage);
                    break;
                case BotEvent.Message:
                    responseMessage = ReactToMessage(eventMessage);
                    break;
                case BotEvent.Ping:
                    responseMessage = ReactToPing(eventMessage);
                    break;
                case BotEvent.UserAddedToConversation:
                    responseMessage = ReactToUserAdded(eventMessage);
                    break;
                case BotEvent.UserRemovedFromConversation:
                    responseMessage = ReactToUserRemoved(eventMessage);
                    break;
                case BotEvent.Unknown:
                default:
                    responseMessage = ReactToUnknown(eventMessage);
                    break;
            }

            responseMessage.Text += 
                ", JSON: " + JsonConvert.SerializeObject(eventMessage);

            return responseMessage;
        }

        Message ReactToBotAdded(Message eventMessage)
        {
            return eventMessage.CreateReplyMessage(
                $"Bot Added From - Address: {eventMessage.From.Address}, Name: {eventMessage.From.Name}");
        }

        Message ReactToBotRemoved(Message eventMessage)
        {
            return eventMessage.CreateReplyMessage(
                $"Bot Removed From - ChanelId: {eventMessage.From.ChannelId}, Name: {eventMessage.From.Name}");
        }

        Message ReactToDeleteUser(Message eventMessage)
        {
            return eventMessage.CreateReplyMessage(
                $"User Deleted From - Address: {eventMessage.From.Address}, Name: {eventMessage.From.Name}");
        }

        Message ReactToEndConversation(Message eventMessage)
        {
            return eventMessage.CreateReplyMessage(
                $"End Conversation From - Address: {eventMessage.From.Address}, Name: {eventMessage.From.Name}");
        }

        Message ReactToMessage(Message eventMessage)
        {
            return eventMessage.CreateReplyMessage(
                $"Message From - Address: {eventMessage.From.Address}, Name: {eventMessage.From.Name}, Text: {eventMessage.Text}");
        }

        Message ReactToPing(Message eventMessage)
        {
            return eventMessage.CreateReplyMessage(
                $"Pingback From - Address: {eventMessage.From.Address}, Name: {eventMessage.From.Name}");
        }

        Message ReactToUserAdded(Message eventMessage)
        {
            return eventMessage.CreateReplyMessage(
                $"User Added From - Address: {eventMessage.From.Address}, Name: {eventMessage.From.Name}");
        }

        Message ReactToUserRemoved(Message eventMessage)
        {
            return eventMessage.CreateReplyMessage(
                $"User removed From - Address: {eventMessage.From.Address}, Name: {eventMessage.From.Name}");
        }

        Message ReactToUnknown(Message eventMessage)
        {
            return eventMessage.CreateReplyMessage(
                "Please forgive me. I'm just a bot and don't understand.");
        }

        BotEvent GetBotEvent(string eventType)
        {
            BotEvent botEvent;

            if (!Enum.TryParse<BotEvent>(eventType, out botEvent))
                botEvent = BotEvent.Unknown;

            return botEvent;
        }
    }
}
