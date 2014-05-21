using System;
using System.Collections.Generic;
using System.Text;

namespace LordDesign.Utilities
{
    public class WebApiCall : ApiCall
    {
        #region Fields

        private bool _disposed;

        #endregion

        #region Constructors and Destructors

        public WebApiCall()
        {
            QueryParams = new Dictionary<string, string>();
            Method = "GET";
        }

        #endregion

        #region Public Properties

        public string Action { get; set; }

        public string Controller { get; set; }

        public string Id { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override IApiResult<dynamic> Execute<T>()
        {
            string endPoint = BuildEndpoint() + BuildQueryString();
            return base.Execute<T>(new Uri(endPoint));
        }

        #endregion

        #region Methods

        protected void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
            {
                return;
            }
            base.Dispose(true);
            _disposed = true;
        }

        private string BuildEndpoint()
        {
            var sb = new StringBuilder(50);
            sb.Append(BaseUrl);

            if (string.IsNullOrWhiteSpace(Controller))
            {
                return sb.ToString();
            }

            if (!BaseUrl.EndsWith("/"))
            {
                sb.Append("/");
            }

            sb.Append(Controller.ToLower());

            if (!Controller.EndsWith("/"))
            {
                sb.Append("/");
            }

            if (!string.IsNullOrWhiteSpace(Id))
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    sb.Append(Id.ToLower());
                }
            }

            if (!string.IsNullOrWhiteSpace(Action))
            {
                sb.Append("/").Append(Action.ToLower());
            }

            return sb.ToString();
        }

        #endregion
    }
}