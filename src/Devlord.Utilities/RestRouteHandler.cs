// -----------------------------------------------------------------------
// <copyright file="RestRouteHandler.cs" company="Lord Design">
//   Copyright (c) 2012 Lord Design, Paradise California.
//   Modified GPL: You can alter it if the result is free. You have permission
//   to incorporate it as-is in a project that is not free.
// </copyright>
// -----------------------------------------------------------------------

namespace Devlord.Utilities
{
    using System.Web;
    using System.Web.Compilation;
    using System.Web.Routing;
    using System.Web.UI;

    public struct RouteMap
    {
        public string PathFormat { get; set; }

        public string TargetPath { get; set; }

        public string DataKey { get; set; }
    }

    public class RestRouteHandler : IRouteHandler
    {
        private readonly string virtualPath;

        private readonly string dataKey;

        public RestRouteHandler(string virtualPath, string dataKey)
        {
            this.virtualPath = virtualPath;
            this.dataKey = dataKey;
        }

        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string path = string.Format(
                "{0}?{1}={2}",
                this.virtualPath,
                this.dataKey,
                context.RouteData.Values[this.dataKey]);
            HttpContext.Current.RewritePath(path);

            var page = BuildManager.CreateInstanceFromVirtualPath(this.virtualPath, typeof(Page)) as IHttpHandler;
            return page;
        }
    }
}
