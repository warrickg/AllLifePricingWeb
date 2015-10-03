using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace AllLifePricingWeb
{
    /// <summary>
    /// Summary description for KeepSessionAlive
    /// </summary>
    public class KeepSessionAlive : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetNoServerCaching();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}