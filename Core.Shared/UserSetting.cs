using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared
{
    public class UserSetting
    {
        public string? InstructionId { get; set; }
        public string? Name { get; set; }
        public string? AppUserId { get; set; }
        public string? InputType { get; set; }
        public string? SubjectId { get; set; }
        public string? Subject { get; set; }
        public string? Language { get; set; }
        public string? Color { get; set; }
        public string? TopicTitle { get; set; }
        public string? IsQueryActive { get; set; }
        public bool IsStreamOutPut { get; set; }
        public string? Prompt { get; set; }
        public Stream? PromptStream { get; set; }
        public string? IsSpeakerEnabled { get; set; }
        public string? IsSaveEnabled { get; set; }
        public string? UserInstructions { get; set; }
        public string? UserDefinedSystemInstructions { get; set; }
        public string? UserBioToUnderstandUser { get; set; }

    }
}
