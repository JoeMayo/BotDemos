﻿using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Web.Http;
using Microsoft.Bot.Builder.Scorables;

namespace BugDialogBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            RegisterScorables();
        }

        void RegisterScorables()
        {
            Conversation.UpdateContainer(builder =>
            {
                builder.RegisterType<HelpScorable>()
                    .As<IScorable<IActivity, double>>()
                    .InstancePerLifetimeScope();
            });
        }
    }
}
