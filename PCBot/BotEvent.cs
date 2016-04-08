using System;
using System.Linq;

namespace PCBot
{
    public enum BotEvent
    {
        Unknown,
        BotAddedToConversation,
        BotRemovedFromConversation,
        DeleteUserData,
        EndOfConversation,
        Message,
        Ping,
        UserAddedToConversation,
        UserRemovedFromConversation
    }
}
