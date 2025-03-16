 
//using Google.Ads.GoogleAds.Lib;
//using Google.Ads.GoogleAds.V14.Common;
//using Google.Ads.GoogleAds.V14.Enums;
//using Google.Ads.GoogleAds.V14.Errors;
//using Google.Ads.GoogleAds.V14.Resources;
//using Google.Api.Gax;
//using System;

//public class GoogleAdsKeywordResearch
//{
//    public void GetKeywordIdeas(string keyword)
//    {
//        // Initialize Google Ads API client
//        //GoogleAdsClient googleAdsClient = GoogleAdsClient.Create();
//        // Create Google Ads API client configuration

//        GoogleAdsConfig googleAdsConfig = new GoogleAdsConfig
//        {
//            DeveloperToken = "YOUR_DEVELOPER_TOKEN",
//            LoginCustomerId = "YOUR_LOGIN_CUSTOMER_ID"
//            // Add any other necessary configuration options here
//        };

//        // Initialize Google Ads API client
//        GoogleAdsClient googleAdsClient = new GoogleAdsClient(googleAdsConfig);

//        // Create a keyword plan service client
//        KeywordPlanIdeaServiceClient keywordPlanIdeaServiceClient =
//            googleAdsClient.GetService(Services.V14.KeywordPlanIdeaService);

//        // Create a keyword plan network
//        KeywordPlanNetworkEnum.Types.KeywordPlanNetwork network =
//            KeywordPlanNetworkEnum.Types.KeywordPlanNetwork.GoogleSearch;

//        // Create a language for the keyword plan
//        string language = "en";

//        // Create a keyword plan idea request
//        GenerateKeywordIdeasRequest request = new GenerateKeywordIdeasRequest
//        {
//            CustomerId = googleAdsClient.Config.ClientCustomerId.ToString(),
//            KeywordPlanNetwork = network,
//            KeywordPlanIdeaSelector = new KeywordPlanIdeaSelector
//            {
//                SeedKeywords =
//                {
//                    new KeywordPlanKeyword
//                    {
//                        Text = keyword,
//                        MatchType = KeywordMatchTypeEnum.Types.KeywordMatchType.Broad
//                    }
//                },
//                Language = language
//            }
//        };

//        try
//        {
//            // Send the request and retrieve the response
//            PagedEnumerable<GenerateKeywordIdeaResult, KeywordPlanIdea> response =
//                keywordPlanIdeaServiceClient.GenerateKeywordIdeas(request);

//            // Process the response
//            foreach (KeywordPlanIdea idea in response)
//            {
//                Console.WriteLine($"Keyword: {idea.Text}, Avg. Monthly Searches: {idea.AvgMonthlySearches}");
//            }
//        }
//        catch (GoogleAdsException e)
//        {
//            Console.WriteLine("Google Ads API request failed:");
//            foreach (GoogleAdsError error in e.Errors)
//            {
//                Console.WriteLine($"Error message: {error.Message}");
//            }
//        }
//    }
//}
