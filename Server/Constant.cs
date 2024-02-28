namespace Server
{
    public class Constant
    {
        public const string ApiBaseUrl = "https://connecttoapi.in/"; //"https://localhost:5001/";
        public const string PromptModel_Text_Davinci_003 = "text-davinci-003";
        public const string PromptModel_GPT_3_5_Turbo = "gpt-3.5-turbo";
        public const string OutputFolderPath = "c:/files/JSONL_Files";
        public const string InputFolderPath = "c:/files/uploads";
        public const string ChatCompletionsApi = "https://api.openai.com/v1/chat/completions";
        public const string FineTuneApi = "https://api.openai.com/v1/files";
        public const string BlobConnectionString = "DefaultEndpointsProtocol=https;AccountName=blobconnect;AccountKey=n9Si1znvSFjOPw1xioHiPq/8w28Cy4CBFKvPKtABOKdyJB7dF7ubZz6wFPiB9zLGmZ5kac4NK/lL+AStGKwUlA==;EndpointSuffix=core.windows.net";
        public const string BlobFilesContainer = "files";
        public const string BlobImagesContainer = "images";
    }
}
