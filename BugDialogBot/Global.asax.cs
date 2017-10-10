using Autofac;
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
            var builder = new ContainerBuilder();

            builder.RegisterType<HelpScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            builder.Update(Conversation.Container);
        }
    }
}
