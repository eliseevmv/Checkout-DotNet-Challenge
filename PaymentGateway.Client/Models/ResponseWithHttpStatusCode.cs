using System.Net;

namespace PaymentGateway.Client.Models
{
    public class ResponseWithHttpStatusCode<T>
    {
        public T Content { get; }
        public HttpStatusCode HttpStatusCode { get; }

        public ResponseWithHttpStatusCode(T content, HttpStatusCode statusCode)
        {
            Content = content;
            HttpStatusCode = statusCode;
        }
    }
}
