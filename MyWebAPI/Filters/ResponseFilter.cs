using MyWebAPI.Models;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace MyWebAPI.Filters
{
    /// <summary>
    /// Response拦截器
    /// </summary>
    public class ResponseFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 重写调用后请求(同步)
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null) return;
            var status = actionExecutedContext.ActionContext.Response.StatusCode;

            object data = null;
            if(actionExecutedContext.ActionContext.Response.Content != null)
            {
                data = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result;
            }
            
            var result = new ResultModel<object>(data);
            var info = $"接口: {FilterUtils.GetActionFullName(actionExecutedContext.ActionContext)}{Environment.NewLine}返回值: {JsonEx.ToJson(result)}";
            Logger.Info(info);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(status, result);

            base.OnActionExecuted(actionExecutedContext);
        }


        /// <summary>
        /// 重写调用后请求(异步)
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

    }
}