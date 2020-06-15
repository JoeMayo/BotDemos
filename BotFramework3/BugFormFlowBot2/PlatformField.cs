using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow.Advanced;

namespace BugFormFlowBot2
{
    public class PlatformField : Field<string>
    {
        public PlatformField(string name, FieldRole role) : base(name, role)
        {
        }
    }
}