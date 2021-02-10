using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace SGE.Web.Infrastucture
{
    public interface IHttpContextService
    {

        HttpContextBase Context { get; }
        HttpRequestBase Request { get; }
        HttpResponseBase Response { get; }
        NameValueCollection FormOrQuerystring { get; }
    }
}