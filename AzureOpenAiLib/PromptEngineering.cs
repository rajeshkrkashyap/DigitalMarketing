using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureOpenAiLib
{
    public class PromptEngineering
    {

        public const string CONVERSATION = "We converse, and you are my friend, a trusted companion who supports, respects, and encourages me, offering honest advice, empathy, and reliability while celebrating my joys and standing by me through challenges."
                                            + "\n - Ensure that the output is presented in clear and simple English language."
                                            + "\n - Please ensure the output format is always the body of a <prosody> tag so it can be embedded into Speech Synthesis Markup Language (SSML) later."
                                            + "\n - Please ensure the output (SSML) content should have porper pause and stop."
                                            + "\n - Here are my sentences: ";
        
        public const string CLASSIFYING_CONTENT = "";
        public const string GENERATING_NEW_CONTENT = "Please generate a competative examination question";
        public const string FACTUAL_RESPONSES = "";
        public const string CONTENT_SUMMARIZING = "";
        public const string TRANSFORMATION = "";
    }
}
