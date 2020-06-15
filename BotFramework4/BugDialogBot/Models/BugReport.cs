using System.Collections.Generic;

namespace BugDialogBot.Models
{
    public class BugReport
    {
        public ProductOptions Product { get; set; }

        public PlatformOptions Platform { get; set; }

        public string Description { get; set; }
    }
}
