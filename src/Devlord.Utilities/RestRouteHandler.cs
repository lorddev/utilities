// -----------------------------------------------------------------------
// <copyright file="RestRouteHandler.cs" company="Lord Design">
//   Copyright © 2012 Lord Design, Paradise California.
//   Modified GPL: You can alter it if the result is free. You have permission
//   to incorporate it as-is in a project that is not free.
// </copyright>
// -----------------------------------------------------------------------


#if NET451
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.UI;

namespace Devlord.Utilities
{
    public class RestRouteHandler : IRouteHandler
    {
        private readonly string _dataKey;
        private readonly string _virtualPath;

        public RestRouteHandler(string virtualPath, string dataKey)
        {
            _virtualPath = virtualPath;
            _dataKey = dataKey;
        }

        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string path = $"{_virtualPath}?{_dataKey}={context.RouteData.Values[_dataKey]}";
            HttpContext.Current.RewritePath(path);

            var page = BuildManager.CreateInstanceFromVirtualPath(_virtualPath, typeof(Page)) as IHttpHandler;
            return page;
        }
    }
}

#endif