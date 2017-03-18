using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Devlord.Utilities
{
    public class ApiCall : IApiCall
    {
        #region Fields

        private readonly HttpClient _client = new HttpClient();

        private readonly JsonSerializerSettings _settings;

        private bool _disposed;

        private string _endPoint;

        #endregion

        #region Constructors and Destructors

        public ApiCall(string endpoint)
            : this()
        {
            QueryParams = new Dictionary<string, string>();
            Method = "GET";
            _endPoint = endpoint;
            _settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        public ApiCall(string endpoint, JsonSerializerSettings settings)
            : this(endpoint)
        {
            _settings = settings;
        }

        public ApiCall(Uri endpoint)
            : this(endpoint.PathAndQuery)
        {
        }

        protected ApiCall()
        {
            _settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        #endregion

        #region Public Properties

        public string BaseUrl { get; set; }

        public string Method { get; set; }

        public object Payload { get; set; }

        public IDictionary<string, string> QueryParams { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual IApiResult<dynamic> Execute<T>() where T : class
        {
            _endPoint += BuildQueryString();
            return Execute<T>(new Uri(_endPoint));
        }

        public virtual IApiResult<dynamic> Execute<T>(Uri endPoint) where T : class
        {
            Func<object, string> serialize;
            string format;
            if (Payload is string)
            {
                serialize = x => x as string;
                format = "application/xml";
            }
            else
            {
                serialize = JsonConvert.SerializeObject;
                format = "application/json";
            }

            HttpResponseMessage response;

            switch (Method.ToUpper().Trim())
            {
                case "PUT":
                {
                    var stringContent = new StringContent(serialize(Payload));
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue(format) { CharSet = "utf-8" };
                    response = _client.PutAsync(_endPoint, stringContent).Result;
                    break;
                }
                case "POST":
                {
                    var stringContent = new StringContent(serialize(Payload));
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue(format) { CharSet = "utf-8" };
                    response = _client.PostAsync(_endPoint, stringContent).Result;
                    break;
                }
                default:
                    // Assume "GET"
                    response = _client.GetAsync(_endPoint).Result;
                    break;
            }

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return new ApiResult<dynamic> { StatusCode = response.StatusCode };
            }

            if (response.Content != null)
            {
                string content = response.Content.ReadAsStringAsync().Result;

                // Assume we know how to deserialize the object.
                if (format.EndsWith("json"))
                {
                    var dataItem = JsonConvert.DeserializeObject<T>(content, _settings);

                    return new ApiResult<dynamic> { StatusCode = response.StatusCode, DataItem = dataItem };
                }

                var contentObject = content.DeserializeAs<T>();

                return new ApiResult<dynamic> { StatusCode = response.StatusCode, DataItem = contentObject };
            }

            return new ApiResult<dynamic> { StatusCode = response.StatusCode };
        }

        #endregion

        #region Methods

        protected string BuildQueryString()
        {
            var sb = new StringBuilder();

            int length = QueryParams.Count;
            int i = 0;
            foreach (var item in QueryParams)
            {
                sb.AppendFormat("{0}={1}", item.Key, item.Value);

                if (i < length - 1)
                {
                    sb.Append("&");
                    i++;
                }
            }

            string qs2 = sb.ToString();
            return "?" + qs2;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _client.Dispose();
                _disposed = true;
            }
        }

        #endregion
    }
}