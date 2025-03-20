using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Extentions
{
    public static class StringExtensions
    {
        public static string TrimTags(this string input, string startTag, string endTag)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.StartsWith(startTag))
            {
                input = input.Substring(startTag.Length);
            }

            if (input.EndsWith(endTag))
            {
                input = input.Substring(0, input.Length - endTag.Length);
            }

            return input;
        }
    }
}
