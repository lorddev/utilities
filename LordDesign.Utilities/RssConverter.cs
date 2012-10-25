// -----------------------------------------------------------------------
// <copyright file="RssConverter.cs" company="Lord Design">
//   Copyright (c) 2012 Lord Design, Paradise California.
//   Modified GPL: You can alter it if the result is free. You have permission
//   to incorporate it as-is in a project that is not free.
// </copyright?
// <created>10/05/2012 1:46 PM</created>
// <author>aaron</author>
// -----------------------------------------------------------------------

namespace LordDesign.Utilities
{
    using System;
    using System.IO;
    using System.Net;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RssConverter
    {
        #region Public Properties

        public string FeedUrl { get; set; }

        public bool HasErrors { get; private set; }

        #endregion

        #region Public Methods and Operators

        public string GetJson(string nodeFilter = null, int maxRecords = 0)
        {
            try
            {
                var xml = this.LoadFeed();
                var filtered = this.ApplyFilter(xml, nodeFilter, maxRecords);
                string json = JsonConvert.SerializeXNode(filtered);
                return json;
            }
            catch (Exception exception)
            {
                this.HandleError(exception);
                return null;
            }
        }

        public AfterFilter PostFilter = x => x;

        public delegate XDocument AfterFilter(XDocument xml);

        #endregion

        #region Methods

        private XDocument ApplyFilter(XDocument xml, string filter, int maxRecords)
        {
            XDocument root = xml;
            if (!string.IsNullOrEmpty(filter))
            {
                var list = xml.Descendants(filter);
                if (maxRecords > 0)
                {
                    list = list.Take(maxRecords);
                }

                root = new XDocument();
                var rss = new XElement("rss");
                root.Add(rss);
                list.ForEach(x => rss.Add(x.RemoveNamespaces()));
                //list.ForEach(rss.Add);
            }

            return PostFilter(root);
        }

        private void HandleError(Exception error)
        {
            this.HasErrors = true;
            Logger.Log(error);
        }

        private XDocument LoadFeed()
        {
            string feed = this.DownloadString();
            feed = CleanFeed(feed);
            var doc = XDocument.Parse(feed);
            return doc;
        }

        //private string CleanFeed(string feed)
        //{
        //    var array = Encoding.UTF8.GetBytes(feed);
        //    using (var input = new MemoryStream())
        //    {
        //        input.Write(array, 0, array.Length);
        //        input.Position = 0;

        //        using (var output = new MemoryStream())
        //        {
        //            var tidy = new Tidy();
        //            var messages = new TidyMessageCollection();
        //            tidy.Parse(input, output, messages);
        //            feed = this.GetString(output);
        //        }

        //        return feed;
        //    }
        //}

        public static string CleanFeed(string feed)
        {
            return feed;
            //using (var doc = Document.FromString(feed))
            //{
            //    doc.ShowWarnings = false;
            //    doc.Quiet = true;
            //    doc.OutputXhtml = true;
            //    doc.CleanAndRepair();
            //    string parsed = doc.Save();
            //    return parsed;
            //}
        }

        private string GetString(MemoryStream stream)
        {
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        private string DownloadString()
        {
            var client = this.MakeClient();
            using (var stream = client.OpenRead(this.FeedUrl))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private WebClient MakeClient()
        {
            var client = new WebClient();
            client.BaseAddress = "http://" + new Uri(this.FeedUrl).GetLeftPart(UriPartial.Authority);
            client.Headers.Add(
                "user-agent",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.13 (KHTML, like Gecko) Chrome/24.0.1284.0 Safari/537.13");
            return client;
        }

        #endregion
    }
}