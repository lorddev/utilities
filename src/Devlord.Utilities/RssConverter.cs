// -----------------------------------------------------------------------
// <copyright file="RssConverter.cs" company="Lord Design">
//   Copyright © 2012 Lord Design, Paradise California.
//   Modified GPL: You can alter it if the result is free. You have permission
//   to incorporate it as-is in a project that is not free.
// </copyright?
// <created>10/05/2012 1:46 PM</created>
// <author>aaron</author>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Devlord.Utilities
{
    /// <summary>
    /// Converts an RSS feed to a JSON string.
    /// </summary>
    public class RssConverter
    {
        #region Public Properties

        public string FeedUrl { get; set; }

        public bool HasErrors { get; private set; }

        #endregion

        #region Public Methods and Operators

        [Obsolete("Use GetJsonAsync() instead.", true)]
        public string GetJson(string nodeFilter = null, int maxRecords = 0)
        {
            return GetJsonAsync(nodeFilter, maxRecords).Result;
        }

        public async Task<string> GetJsonAsync(string nodeFilter = null, int maxRecords = 0)
        {
            try
            {
                var xml = await LoadFeed();
                var filtered = ApplyFilter(xml, nodeFilter, maxRecords);
                var json = JsonConvert.SerializeXNode(filtered);
                return json;
            }
            catch (Exception exception)
            {
                HandleError(exception);
                return null;
            }
        }

        public AfterFilter PostFilter = x => x;

        public delegate XDocument AfterFilter(XDocument xml);

        #endregion

        #region Methods

        private XDocument ApplyFilter(XDocument xml, string filter, int maxRecords)
        {
            var root = xml;
            if (string.IsNullOrEmpty(filter))
            {
                return PostFilter(root);
            }

            var list = xml.Descendants(filter);
            if (maxRecords > 0)
            {
                list = list.Take(maxRecords);
            }

            root = new XDocument();
            var rss = new XElement("rss");
            root.Add(rss);
            list.ForEach(x => rss.Add(x.RemoveNamespaces()));

            return PostFilter(root);
        }

        private void HandleError(Exception error)
        {
            HasErrors = true;
            Logger.Log(error);
        }

        private async Task<XDocument> LoadFeed()
        {
            var feed = await DownloadString();
            var doc = XDocument.Parse(feed);
            return doc;
        }

        [Obsolete("This feature has been deprecated. I don't remember why, sorry.")]
        public static string CleanFeed(string feed)
        {
            return feed;
        }

        private async Task<string> DownloadString()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("http://" + new Uri(FeedUrl)
                                      .GetComponents(UriComponents.StrongAuthority, UriFormat.Unescaped));
                var request = new HttpRequestMessage
                {
                    RequestUri = uri,
                    Method = HttpMethod.Get
                };

                request.Headers.Add(
                    "user-agent",
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.13 (KHTML, like Gecko) Chrome/24.0.1284.0 Safari/537.13");

                using (var response = await client.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        #endregion
    }
}