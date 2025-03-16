using Core.Service.SeoScore;
using Core.Shared;
using Core.Shared.Entities;
using HtmlAgilityPack;
using System.Net.Http;
using System.Reflection;
using Core.Service.Azure;
namespace ServerLib.SeoScore
{
    /// <summary>
    /// The on-page SEO score of a website is influenced by various factors that search 
    /// engines consider when evaluating the relevance and quality of a web page. 
    /// While different tools and platforms may use slightly different criteria, common 
    /// parameters to calculate on-page SEO score typically include:
    /// </summary>
    public class OnPageSeoScore
    {
        private static IOnPageSeoScore? keywordUsageModel;
        private static IOnPageSeoScore? contentQualityModel;
        private static IOnPageSeoScore? metaTagModel;
        private static IOnPageSeoScore? imagesAndMultimediaModel;
        private static IOnPageSeoScore? internalLinkingModel;
        private static IOnPageSeoScore? uRLStructureModel;
        private static IOnPageSeoScore? pageLoadingSpeedModel;
        private static IOnPageSeoScore? mobileFriendlinessModel;
        private static IOnPageSeoScore? socialSignalModel;
        private static IOnPageSeoScore? technicalSEOModel;
        private static IOnPageSeoScore? securityModel;

        private static readonly object LockObject = new object();
        static OnPageSeoScore()
        {
            HtmlDocument document = new HtmlDocument();

            keywordUsageModel = new KeywordUsageModel(document, new SeoScoreBase<string, List<KeywordUsage>>()); //DONE

            contentQualityModel = new ContentQualityModel(document, new AzureOpenAiService(),
                                  new SeoScoreBase<string, ContentQuality>(),
                                  new SeoScoreBase<string, RelevanceToTheTopic>(),
                                  new SeoScoreBase<string, AccuracyAndCredibility>(),
                                  new SeoScoreBase<string, ClarityAndReadability>(),
                                  new SeoScoreBase<string, DepthAndBreadthOfCoverage>(),
                                  new SeoScoreBase<string, ReputationAndAuthority>(),
                                  new SeoScoreBase<string, PurposeAndIntent>()); //DONE

            metaTagModel = new MetaTagModel(document, new AzureOpenAiService(), new SeoScoreBase<string, MetaTag>()); //DONE

            imagesAndMultimediaModel = new ImagesAndMultimediaModel(document, new AzureOpenAiService(), new SeoScoreBase<string, ImagesAndMultimedia>()); // Done

            internalLinkingModel = new InternalLinkingModel(document, new AzureOpenAiService(), new SeoScoreBase<string, InternalLinking>()); // DONE

            uRLStructureModel = new URLStructureModel(document, new AzureOpenAiService(), new SeoScoreBase<string, URLStructure>()); // DONE

            pageLoadingSpeedModel = new PageLoadingSpeedModel(document, new AzureOpenAiService(), new SeoScoreBase<string, PageLoadingSpeed>()); // DONE

            mobileFriendlinessModel = new MobileFriendlinessModel(document, new AzureOpenAiService(), new SeoScoreBase<string, MobileFriendliness>()); // DONE

            socialSignalModel = new SocialSignalModel(document, new AzureOpenAiService(), new SeoScoreBase<string, SocialSignal>());
            technicalSEOModel = new TechnicalSEOModel(document, new AzureOpenAiService(), new SeoScoreBase<string, TechnicalSEO>());
            securityModel = new SecurityModel(document, new AzureOpenAiService(), new SeoScoreBase<string, Security>());
        }

        public static void ProcessSeoScore(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlContent, string crawledId, string _seedUrl = null)
        {
            try
            {
                lock (LockObject)
                {
                    keywordUsageModel?.Process(document, project, ignoreWordList, htmlContent, crawledId);
                    contentQualityModel?.Process(document,project, ignoreWordList, htmlContent, crawledId);
                    metaTagModel?.Process(document, project, ignoreWordList, htmlContent, crawledId);
                    imagesAndMultimediaModel?.Process(document, project, ignoreWordList, htmlContent, crawledId);
                    internalLinkingModel?.Process(document, project, ignoreWordList, htmlContent, crawledId);
                    uRLStructureModel?.Process(document, project, ignoreWordList, htmlContent, crawledId, _seedUrl);
                    pageLoadingSpeedModel?.Process(document, project, ignoreWordList, htmlContent, crawledId, _seedUrl);
                    mobileFriendlinessModel?.Process(document, project, ignoreWordList, htmlContent, crawledId, _seedUrl);
                    socialSignalModel?.Process(document, project, ignoreWordList, htmlContent, crawledId, _seedUrl);
                    technicalSEOModel?.Process(document, project, ignoreWordList, htmlContent, crawledId, _seedUrl);
                    securityModel?.Process(document, project, ignoreWordList, htmlContent, crawledId, _seedUrl);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
