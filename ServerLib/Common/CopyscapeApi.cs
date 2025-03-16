using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace ServerLib.Common
{
    public class CopyscapeApi
    {
        /*
	ASP.NET C# sample code for Copyscape Premium API
	
	Compatible with ASP.NET 2.0 or later
	
	You may install, use, reproduce, modify and redistribute this code, with or without
	modifications, subject to the general Terms and Conditions on the Copyscape website. 
	
	For any technical assistance please contact us via our website.
	
	07-May-2013: First version
	
	Copyscape (c) Indigo Stream Technologies 2013 - http://www.copyscape.com/


	Instructions for use:
	
    1. Set the constants COPYSCAPE_USERNAME and KEY below to your details.
    2. Call the appropriate API function, following the examples below.
    3. The API response is in XML, which in this sample code is parsed and returned as an XMLElement.
    4. To run the examples provided, please set run_examples in the next line to true:
*/

        public static bool run_examples = false;

        /*
            Error handling:

            * If a call failed completely (e.g. HttpWebRequest failed to connect), functions return null.
            * If the API returned an error, the response XmlElement will contain an 'error' node.
        */

        /*
            A. Constants you need to change
        */
        const string COPYSCAPE_USERNAME = "rajeshx21";
        const string KEY = "y2bw93sqebqn0hxl";
        const string URL = "http://www.copyscape.com/api/";

        /*
            B. static public functions for you to use (all accounts)
        */

        public static XmlElement example_search()
        {
            Dictionary<string, string> urlparams = new Dictionary<string, string>();
            urlparams.Add("x", "1");
            return call("csearch", urlparams, null);
        }
        public static XmlElement url_search_internet(string url, int full)
        {
            return url_search(url, full, "csearch");
        }
        public static XmlElement text_search_internet(string text, int full)
        {
            return text_search(text, "ISO-8859-1", full, "csearch");
        }
        public static XmlElement check_balance()
        {
            return call("balance", null, null);
        }

        /*
            C. static public functions for you to use (only accounts with private index enabled)
        */

        public static XmlElement url_search_private(string url, int full)
        {
            return url_search(url, full, "psearch");
        }
        public static XmlElement url_search_internet_and_private(string url, int full)
        {
            return url_search(url, full, "cpsearch");
        }
        public static XmlElement text_search_private(string text, string encoding, int full)
        {
            return text_search(text, encoding, full, "psearch");
        }
        public static XmlElement text_search_internet_and_private(string text, string encoding, int full)
        {
            return text_search(text, encoding, full, "cpsearch");
        }
        public static XmlElement url_add_to_private(string url, string id)
        {
            Dictionary<string, string> urlparams = new Dictionary<string, string>();
            urlparams.Add("q", url);

            if (id != null)
                urlparams.Add("i", id);

            return call("pindexadd", urlparams, null);
        }
        public static XmlElement text_add_to_private(string text, string encoding, string title, string id)
        {
            Dictionary<string, string> urlparams = new Dictionary<string, string>();
            urlparams.Add("e", encoding);

            if (title != null)
                urlparams.Add("a", title);

            if (id != null)
                urlparams.Add("i", id);

            return call("pindexadd", urlparams, text);
        }
        public static XmlElement delete_from_private(string handle)
        {
            Dictionary<string, string> urlparams = new Dictionary<string, string>();
            urlparams.Add("h", handle);

            return call("pindexdel", urlparams, null);
        }
        public static String wrap_title(string title)
        {
            //return "<big style='margin-left:5%'><b>" + HttpContext.Current.Server.HtmlEncode(title) + ":</b></big>";
            return "";
        }
        public static String wrap_node(XmlNode node)
        {
            //return "<div style='overflow:auto; max-height:300px; margin-left:5%; width:90%'><PRE>" + HttpContext.Current.Server.HtmlEncode(node_recurse(node, 0)) + "</PRE></div><br>";
            return "";
        }
        private static string node_recurse(XmlNode node, int depth)
        {
            string ret = "";

            if (node == null)
                return ret;

            if (node.NodeType == XmlNodeType.Text)
                ret += node.Value;
            else
                ret += "\n" + new string('\t', depth) + node.Name + ": ";

            if (node.HasChildNodes)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                    ret += node_recurse(node.ChildNodes[i], depth + 1);
            }

            return ret;
        }

        /*
            D. Functions used internally
        */

        private static  XmlElement url_search(string url, int full, string operation)
        {
            Dictionary<string, string> urlparams = new Dictionary<string, string>();
            urlparams.Add("q", url);

            if (full != 0)
                urlparams.Add("c", full.ToString());

            return call(operation, urlparams, null);
        }
        private static  XmlElement text_search(string text, string encoding, int full, string operation)
        {
            Dictionary<string, string> urlparams = new Dictionary<string, string>();
            urlparams.Add("e", encoding);

            if (full != 0)
                urlparams.Add("c", full.ToString());

            return call(operation, urlparams, text);
        }
        private static  XmlElement call(string operation, Dictionary<string, string> urlparams, string postdata)
        {
            string url = URL + "?u=" + HttpUtility.UrlEncode(COPYSCAPE_USERNAME) +
                "&k=" + HttpUtility.UrlEncode(KEY) + "&o=" + HttpUtility.UrlEncode(operation);

            if (urlparams != null)
            {
                foreach (KeyValuePair<string, string> kvp in urlparams)
                    url += ("&" + HttpUtility.UrlEncode(kvp.Key) + "=" + HttpUtility.UrlEncode(kvp.Value));
            }

            Uri uri = new Uri(url);
            string output = null;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = (postdata == null) ? WebRequestMethods.Http.Get : WebRequestMethods.Http.Post;
            request.ContentType = "application/x-www-form-urlencoded";

            if (postdata != null)
            {
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.UTF8);
                writer.Write(postdata);
                writer.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            output = reader.ReadToEnd();
            response.Close();

            if (output.Length > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(output);
                return (XmlElement)doc.SelectSingleNode("/*");
            }

            return null;
        }
    }
}
