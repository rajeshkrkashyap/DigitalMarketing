using Core.Service.Azure;
using Core.Service.SeoScore;
using Core.Shared.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.SeoScore
{
    public class SecurityModel : BaseModel
    {
        HtmlDocument doc = null;
        private readonly AzureOpenAiService azureOpenAiService;
        private readonly ISeoScore<string, Security>? securityService;
        public SecurityModel(HtmlDocument document, AzureOpenAiService azureAiService,
            SeoScoreBase<string, Security> security) : base(document)
        {
            azureOpenAiService = azureAiService;
            securityService = security;
        }

        /// <summary>
        /// Security:
        /// Use of HTTPS to ensure a secure connection, which is also a ranking factor.
        /// </summary>
        public Security? Security { get; set; }

        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string _seedUrl)
        {
            doc = document;
            if (doc != null)
            {
                //doc.Load(htmlDocument);
            }
        }
    }
}
