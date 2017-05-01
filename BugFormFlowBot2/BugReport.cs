using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System.Threading.Tasks;

namespace BugFormFlowBot2
{
    [Serializable]
    [Template(TemplateUsage.EnumSelectOne, "Which {&} were you working with? {||}")]
    [Template(TemplateUsage.EnumSelectMany, "Which {&} were you working with? {||}", ChoiceStyle = ChoiceStyleOptions.PerLine)]
    public class BugReport
    {
        public string Product { get; set; }

        public string Version { get; set; }

        public List<PlatformOptions> Platform { get; set; }

        [Prompt("What is the {&}")]
        [Describe("Description of the Problem")]
        public string ProblemDescription { get; set; }

        [Numeric(1, 3)]
        public int Priority { get; set; }

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
                    .Field(nameof(Version),
                        validate: async (state, response) =>
                        {
                            var result = new ValidateResult { IsValid = true, Value = response };

                            foreach (var segment in (response as string ?? "").Split('.'))
                            {
                                int digit;
                                if (!int.TryParse(segment, out digit))
                                {
                                    result.Feedback =
                                        "Version number must be numeric segments, optionally separated by dots. e.g. 7.2, 10, or 3.56";
                                    result.IsValid = false;
                                    break;
                                }
                            }

                            return await Task.FromResult(result);
                        })
                    .Field(nameof(Platform))
                    .AddRemainingFields()
                    .Confirm(async (bugReport) =>
                     {
                         var response = new PromptAttribute(
                             $"You entered {bugReport.Product}, {bugReport.Version}, {bugReport.Platform}" +
                             $"{bugReport.ProblemDescription}, {bugReport.Priority}. Is this Correct?");
                         return await Task.FromResult(response);
                     })
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
