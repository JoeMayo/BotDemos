using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace PCBot
{
    public class MessageHandler
    {
        const string UserInfoProperty = "UserInfo";

        public async Task<Activity> ReactAsync(Activity activity)
        {
            Activity responseActivity;

            switch (activity.Type)
            {
                case ActivityTypes.DeleteUserData:
                    responseActivity = await ReactToDeleteUserAsync(activity);
                    break;
                case ActivityTypes.ConversationUpdate:
                    responseActivity = await ReactToConversationUpdateAsync(activity);
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

            return responseActivity;
        }

        async Task<Activity> ReactToDeleteUserAsync(Activity activity)
        {
            StateClient stateClient = activity.GetStateClient();
            string[] response = await stateClient.BotState.DeleteStateForUserAsync(activity.ChannelId, activity.From.Id);
            

            return activity.CreateReply(
                $"User Deleted From - ID: {activity.From.Id}, Name: {activity.From.Name}");
        }

        /// <summary>
        /// Responds to user if this is a new conversation.
        ///   - Says "Hi" if new user.
        ///   - Says "Welcome back" if known user.
        /// </summary>
        /// <param name="activity">Information on who joined conversation.</param>
        /// <returns>"Activity with response or null to not respond.</returns>
        async Task<Activity> ReactToConversationUpdateAsync(Activity activity)
        {
            StateClient stateClient = activity.GetStateClient();
            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

            UserInfo user;

            if (userData.Data != null)
            {
                user = userData.GetProperty<UserInfo>(UserInfoProperty);
            }
            else
            {
                user = new UserInfo
                {
                    UserName = activity.From.Name,
                    Joined = DateTime.Now
                };
            }

            // default response is null, indicating that we shouldn't reply.
            Activity returnActivity = null;

            // if no record of previous visit, treat it as the first time.
            bool IsFirstVisit(UserInfo usr) => usr.LastVisit == default(DateTime);

            if (IsFirstVisit(user))
            {
                bool IsChatbot(ChannelAccount channelAcct) => channelAcct.Id == activity.Recipient.Id;

                if (activity.MembersAdded?.Any(IsChatbot) ?? false)
                    returnActivity = activity.CreateReply("Hi, welcome to PC Bot.");
            }
            else
            {
                returnActivity = activity.CreateReply("Welcome back.");
            }

            user.LastVisit = DateTime.Now;
            userData.SetProperty<UserInfo>(UserInfoProperty, user);

            try
            {
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            }
            catch (HttpOperationException)
            {
                userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                user = userData.GetProperty<UserInfo>(UserInfoProperty);
                user.LastVisit = DateTime.Now;
                userData.SetProperty<UserInfo>(UserInfoProperty, user);
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            }

            return returnActivity;
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
