using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Dialogs;

namespace BugFormFlowBot1
{
    [Serializable]
    public class BugReport
    {
        public List<ProductOptions> Product { get; set; }

        public List<PlatformOptions> Platform { get; set; }

        public string ProblemDescription { get; set; }

        public static IForm<BugReport> BuildForm()
        {
            return new FormBuilder<BugReport>()
                    .Message("Welcome to Bug Report bot!")
                    .Build();
        }
    }
}
