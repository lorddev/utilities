using System.Net;

namespace LordDesign.Utilities
{
    public interface IApiResult<T>
    {
        #region Public Properties

        T DataItem { get; set; }

        HttpStatusCode StatusCode { get; set; }

        #endregion
    }

    public class ApiResult<T> : IApiResult<T>
    {
        #region Public Properties

        public T DataItem { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        #endregion
    }
}