// -----------------------------------------------------------------------
// <copyright file="RestRouteHandler.cs" company="Lord Design">
//   Copyright © 2012 Lord Design, Paradise California.
//   Modified GPL: You can alter it if the result is free. You have permission
//   to incorporate it as-is in a project that is not free.
// </copyright>
// -----------------------------------------------------------------------

namespace Devlord.Utilities
{
    public struct RouteMap
    {
        public string PathFormat { get; set; }

        public string TargetPath { get; set; }

        public string DataKey { get; set; }
    }
}