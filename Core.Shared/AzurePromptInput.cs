using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared
{
    public class AzurePromptInput
    {
        public string? UserMessagePrompt { get; set; }
        public string? ResponseFormat { get; set; }
        public string? DeploymentName { get; set; }
        public string? UserId { get; set; }
    }
}
