using System;
using System.Collections.Generic;
using System.Web;

namespace App.Membership.Services
{
    public static class UrlUtils
    {

        public static string AddParamsToUrl(string url, IEnumerable<KeyValuePair<string, string>> queryParams)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (KeyValuePair<string, string> item in queryParams)
            {
                query.Add(item.Key, item.Value);
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        public static string RemoveParamFromUrl(string url, string queryParamName)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Remove(queryParamName);
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }


        public static string GetRootUrl(string url)
        {
            Uri uri = new Uri(url);
            return string.Format("{0}://{1}:{2}", uri.Scheme, uri.Host, uri.Port);
        }
    }
}
