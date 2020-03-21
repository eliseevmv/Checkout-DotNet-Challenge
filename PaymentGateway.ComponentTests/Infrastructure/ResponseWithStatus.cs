using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PaymentGateway.ComponentTests.Infrastructure
{
    public class ResponseWithStatus<T>
    {
        public T ResponseBody { get; set; }
        public HttpStatusCode StatusCode { get; set; }  
    }
}
