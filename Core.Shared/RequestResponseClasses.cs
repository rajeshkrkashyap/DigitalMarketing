using Azure.AI.OpenAI;
using Core.Shared.Entities;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Shared
{
    #region GPT Request Class structure
    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }
    public class PromptInput
    {
        public string model { get; set; } = PromtModels.GPT_3_5_Turbo;
        public float temperature { get; set; } = (float)0.7;
        public bool stream { get; set; } = false;
        public int max_tokens { get; set; } = 3096; //this is the output/response max length and reserved 1000 token for input and instructions
        public int presence_penalty { get; set; } = 0;
        public int frequency_penalty { get; set; } = 0;
        public string user { get; set; } = "";
        public List<Message> messages { get; set; }
    }
    public class ClientPromptInput
    {
        public string? Language { get; set; }
        public string? InstructionId { get; set; }
        public string? Name { get; set; }
        public string? InputType { get; set; }
        public string? UserId { get; set; }
        public string? Prompt { get; set; }
        public bool IsStreamOutPut { get; set; } = false;
        public string? Subject { get; set; }
        public string? ChildInstructionId { get; set; }
        public string? UserDefinedSystemInstructions { get; set; }
        public string? UserBioToUnderstandUser { get; set; }
    }
    #endregion

    #region GPT Reponse Class structure
    public class ChatCompletion
    {
        public string id { get; set; }
        public string @object { get; set; }
        public long created { get; set; }
        public string model { get; set; }
        public Chatchoice[] choices { get; set; }
        public Usage usage { get; set; }
        public ChatCompletion ChatCompletions { get; set; }
    }
    public class Chatchoice
    {
        public int index { get; set; }
        public Chatdata message { get; set; }
        public Chatdata delta { get; set; }
        public string finish_reason { get; set; }
    }
    public class Chatdata
    {
        public string role { get; set; }
        public string content { get; set; }
    }
    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
    #endregion

    #region Image Prompt Request
    public class ImagePrompt
    {
        public string prompt { get; set; }
        public int n { get; set; } = 1;
        public string size { get; set; } = "256x256";
    }
    #endregion

    #region Image Prompt Reponse
    public class ImageOutput
    {
        public int created { get; set; }
        public List<ImageURL> data { get; set; }

    }
    public class ImageURL
    {
        public string url { get; set; }
    }
    #endregion

    #region MathPix Input Request
    public class MathPixRequest
    {
        public MathPixRequest()
        {
            this.data_options = new DataOptions();
        }

        public string src { get; set; }
        public string[] formats { get; set; }
        public DataOptions data_options { get; set; }
    }
    public class DataOptions
    {
        public bool include_asciimath { get; set; }
    }
    #endregion

    #region MathPix Response output
    public class MathOCRResponse
    {
        public string RequestId { get; set; }
        public string Version { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public bool IsPrinted { get; set; }
        public bool IsHandwritten { get; set; }
        public double AutoRotateConfidence { get; set; }
        public int AutoRotateDegrees { get; set; }
        public double Confidence { get; set; }
        public double ConfidenceRate { get; set; }
        public string Text { get; set; }
        public List<MathOCRData> Data { get; set; }
    }

    public class MathOCRData
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
    #endregion

    //Image data Response 
    public class ReadImageDataResponse
    {
        public bool success { get; set; }
        public string? blobPath { get; set; }
        public string? imageContent { get; set; }
    }

}
