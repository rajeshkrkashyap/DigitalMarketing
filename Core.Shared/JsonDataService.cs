using Microsoft.Extensions.Options;

namespace Core.Shared
{
    public class Settings
    {
        public string? ApiBaseUrl { get; set; } = string.Empty;
        public string? PromptModel_Text_Davinci_003 { get; set; } = string.Empty;
        public string? PromptModel_GPT_3_5_Turbo { get; set; } = string.Empty;
        public string? OutputFolderPath { get; set; } = string.Empty;
        public string? InputFolderPath { get; set; } = string.Empty;
        public string? ChatCompletionsApi { get; set; } = string.Empty;
        public string? FineTuneApi { get; set; } = string.Empty;
        public string? BlobConnectionString { get; set; } = string.Empty;
        public string? BlobFilesContainer { get; set; } = string.Empty;
        public string? BlobImagesContainer { get; set; } = string.Empty;
        public string? AppRootFolderpath { get; set; } = string.Empty;
    }
    public class ConfigService
    {
        IOptions<Settings> _options;
        public ConfigService(IOptions<Settings> options)
        {
            _options = options;
        }

        private Settings? settings;
        public async Task LoadDataAsync(string appPath)
        {
            settings = _options.Value;
            settings.AppRootFolderpath = appPath;
        }

        public Settings? AppSettings
        {
            get
            {
                return _options.Value;
            }
        }
    }
}
