using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MyWebAPI.Models;

namespace MyWebAPI.Filters
{
    /// <summary>
    /// 统一API返回值处理
    /// </summary>
    public class RequestFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 重写调用前请求(同步)
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }
    }
}