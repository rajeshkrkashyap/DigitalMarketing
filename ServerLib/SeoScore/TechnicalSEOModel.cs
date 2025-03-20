using Core.Service.Azure;
using Core.Service.OnPageSeoScore;
using Core.Shared.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.SeoScore
{
    public class TechnicalSEOModel : BaseModel
    {
        HtmlDocument doc = null;
        private readonly AzureOpenAiService azureOpenAiService;
        private readonly ISeoScore<string, TechnicalSEO>? technicalSEOService;
        public TechnicalSEOModel(HtmlDocument document, AzureOpenAiService azureAiService,
            SeoScoreBase<string, TechnicalSEO> technicalSEO) : base(document)
        {
            azureOpenAiService = azureAiService;
            technicalSEOService = technicalSEO;
        }

 

        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string seedUrl)
        { 
            doc= document;
            if (doc != null)
            {
                //doc.Load(htmlDocument);
            }
        }

    }
}
