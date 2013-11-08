using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace LordDesign.Utilities
{
    public class ApiCall
    {
        public ApiCall()
        {
            QueryParams = new Dictionary<string, string>();
            Method = "GET";
        }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Id { get; set; }

        public string BaseUrl { get; set; }

        public string Method { get; set; }

        public object Payload { get; set; }

        public IDictionary<string, string> QueryParams { get; set; }

        public IApiResult<dynamic> Execute<T>() where T : class
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

            using (var httpClient = new HttpClient())
            {
                var endpoint = BuildEndpoint() + BuildQueryString();
                HttpResponseMessage response;

                switch (Method.ToUpper().Trim())
                {
                    case "PUT":
                        {
                            var stringContent = new StringContent(serialize(Payload));
                            stringContent.Headers.ContentType = new MediaTypeHeaderValue(format)
                                {
                                    CharSet = "utf-8"
                                };
                            response = httpClient.PutAsync(endpoint, stringContent).Result;
                            break;
                        }
                    case "POST":
                        {
                            var stringContent = new StringContent(serialize(Payload));
                            stringContent.Headers.ContentType = new MediaTypeHeaderValue(format)
                                {
                                    CharSet = "utf-8"
                                };
                            response = httpClient.PostAsync(endpoint, stringContent).Result;
                            break;
                        }
                    default:
                        // Assume "GET"
                        response = httpClient.GetAsync(endpoint).Result;
                        break;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return new ApiResult<dynamic> { StatusCode = response.StatusCode };
                }

                if (response.Content != null)
                {
                    var content = response.Content.ReadAsStringAsync().Result;

                    // Assume we know how to deserialize the object.
                    if (format.EndsWith("json"))
                    {
                        var dataItem = JsonConvert.DeserializeObject<T>(content);

                        return new ApiResult<dynamic>
                            {
                                StatusCode = response.StatusCode,
                                DataItem = dataItem
                            };
                    }

                    var contentObject = content.DeserializeAs<T>();

                    return new ApiResult<dynamic>
                        {
                            StatusCode = response.StatusCode,
                            DataItem = contentObject
                        };
                }

                return new ApiResult<dynamic> { StatusCode = response.StatusCode };
            }
        }

        private string BuildEndpoint()
        {
            var sb = new StringBuilder(50);
            sb.Append(BaseUrl);
            if (!BaseUrl.EndsWith("/"))
            {
                sb.Append("/");
            }

            sb.Append(Controller.ToLower());

            if (!Controller.EndsWith("/"))
            {
                sb.Append("/");
            }

            if (!string.IsNullOrEmpty(Id))
            {
                sb.Append(Id.ToLower());
            }

            if (!string.IsNullOrEmpty(Action))
            {
                sb.Append("/").Append(Action.ToLower());
            }

            return sb.ToString();
        }

        private string BuildQueryString()
        {
            StringBuilder sb = new StringBuilder();

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
    }

    public interface IApiResult<T>
    {
        HttpStatusCode StatusCode { get; set; }
        T DataItem { get; set; }
    }

    public class ApiResult<T> : IApiResult<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public T DataItem { get; set; }
    }
}
