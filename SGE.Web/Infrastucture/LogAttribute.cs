using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGE.Web.Infrastucture
{
    /// <summary>
    /// This is for Filter when pass for any action
    /// </summary>
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Here capture the error for Log filterContext.exception and put in Log
            // I can Put the error in controller as atributte like [Log] or in Global.asax on RegisterGlobalFilters for every time
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
           base.OnResultExecuted(filterContext);
        }
    }
}