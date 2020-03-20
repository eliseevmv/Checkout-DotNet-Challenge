using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PaymentGateway.IntegrationTests.ServiceClient
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
