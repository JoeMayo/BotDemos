using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System.Threading.Tasks;

namespace BugFormFlowBot2
{
    [Serializable]
    public class BugReport
    {
        public string Product { get; set; }

        public List<PlatformOptions> Platform { get; set; }

        public string ProblemDescription { get; set; }

        public static IForm<BugReport> BuildForm()
        {
            return new FormBuilder<BugReport>()
                    .Message("Welcome to Bug Report bot!")
                    .Field(new FieldReflector<BugReport>(nameof(Product))
                            .SetType(null)
                            .SetDefine((state, field) =>
                            {
                                foreach (var prod in GetProducts())
                                    field
                                        .AddDescription(prod, prod)
                                        .AddTerms(prod, prod);

                                return Task.FromResult(true);
                            }))
                    .Field(nameof(Platform))
                    .AddRemainingFields()
                    .OnCompletion(async (context, bugReport) => 
                     {
                        await context.PostAsync("Thanks for the report!");
                     })
                    .Build();
        }

        static List<string> GetProducts()
        {
            return new List<string>
            {
                "Office",
                "SQL Server",
                "Visual Studio"
            };
        }
    }
}
