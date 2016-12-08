using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace PCBot
{
    public class MessageHandler
    {
        public Activity React(Activity activity)
        {
            Activity responseActivity = null;

            switch (activity.Type)
            {
                case ActivityTypes.DeleteUserData:
                    responseActivity = ReactToDeleteUser(activity);
                    break;
                case ActivityTypes.ConversationUpdate:
                    responseActivity = ReactToConversationUpdate(activity);
                    break;
                case ActivityTypes.ContactRelationUpdate:
                    responseActivity = ReactToContactRelationUpdate(activity);
                    break;
                case ActivityTypes.Message:
                    responseActivity = ReactToMessage(activity);
                    break;
                case ActivityTypes.Ping:
                    responseActivity = ReactToPing(activity);
                    break;
                case ActivityTypes.Typing:
                    responseActivity = ReactToUserRemoved(activity);
                    break;
                default:
                    responseActivity = ReactToUnknown(activity);
                    break;
            }

            responseActivity.Text += 
                ", JSON: " + JsonConvert.SerializeObject(activity);

            return responseActivity;
        }

        Activity ReactToDeleteUser(Activity activity)
        {
            return activity.CreateReply(
                $"User Deleted From - ID: {activity.From.Id}, Name: {activity.From.Name}");
        }

        Activity ReactToConversationUpdate(Activity activity)
        {
            return activity.CreateReply(
                $"Conversation Update From - " +
                $"# Added: {activity.MembersAdded?.Count ?? 0}, " +
                $"# Removed: {activity.MembersRemoved?.Count ?? 0}");
        }

        Activity ReactToContactRelationUpdate(Activity activity)
        {
            return activity.CreateReply(
                $"Bot {activity.Action}ed");
        }

        Activity ReactToMessage(Activity activity)
        {
            return activity.CreateReply(
                $"Message From - ID: {activity.From.Id}, Name: {activity.From.Name}, Text: {activity.Text}");
        }

        Activity ReactToPing(Activity activity)
        {
            return activity.CreateReply(
                $"Pingback From - ID: {activity.From.Id}, Name: {activity.From.Name}");
        }

        Activity ReactToUserRemoved(Activity activity)
        {
            return activity.CreateReply(
                $"I see that you're typing. I'll be annoying and interrupt you.");
        }

        Activity ReactToUnknown(Activity activity)
        {
            return activity.CreateReply(
                text: "Please forgive me. I'm just a bot and don't understand.");
        }
    }
}
