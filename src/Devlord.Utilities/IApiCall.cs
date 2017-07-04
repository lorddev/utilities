using System;
using System.Collections.Generic;

namespace Devlord.Utilities
{
    public interface IApiCall : IDisposable
    {
        IApiResult<dynamic> Execute<T>() where T : class;

        IDictionary<string, string> QueryParams { get; set; }
    }
}