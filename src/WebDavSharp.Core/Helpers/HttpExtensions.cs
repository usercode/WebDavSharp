using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebDavSharp.Core
{
    public static class HttpExtensions
    {
        public static async Task<string> ReadContentAsString(this HttpRequest request)
        {
            StreamReader reader = new StreamReader(request.Body);

            string content = await reader.ReadToEndAsync();

            return content;
        }

        public static string UrlDecode(this string value)
        {
            return HttpUtility.UrlDecode(value);
        }
    }
}
