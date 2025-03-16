using Core.Shared.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.SeoScore
{
    public interface IOnPageSeoScore
    {
        void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string seedUrl= null);
    }
}
