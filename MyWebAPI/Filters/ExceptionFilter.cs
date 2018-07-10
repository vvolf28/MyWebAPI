using MyWebAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace MyWebAPI.Filters
{
    /// <summary>
    /// 异常拦截器
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写异常处理机制(同步)
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            this.LoggerException(context);
            base.OnException(context);
        }

        /// <summary>
        /// 重写异常处理机制(异步)
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            this.LoggerException(actionExecutedContext);
            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }


        private void LoggerException(HttpActionExecutedContext actionExecutedContext)
        {
            var info = $"接口: {FilterUtils.GetActionFullName(actionExecutedContext.ActionContext)}{Environment.NewLine}输入参数: {JsonEx.ToJson(actionExecutedContext.ActionContext.ActionArguments)}";
            Logger.Error(info, actionExecutedContext.Exception);
            var result = new ResultModel<Exception>(actionExecutedContext.Exception);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}