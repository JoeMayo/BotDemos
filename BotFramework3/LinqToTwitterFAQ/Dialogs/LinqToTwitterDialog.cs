using System;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;

namespace LinqToTwitterFAQ.Dialogs
{
    [Serializable]
    [QnAMaker(
        subscriptionKey: "<Your Subscription Key Goes Here>",
        knowledgebaseId: "<Your Knowledge Base ID Goes Here>")]
    public class LinqToTwitterDialog : QnAMakerDialog
    {
    }
}