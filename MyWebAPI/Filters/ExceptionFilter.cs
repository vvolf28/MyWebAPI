using MyWebAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace MyWebAPI.Filters
{
    /// <summary>
    /// 全局异常拦截器
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写异常处理机制(同步)
        /// </summary>
        /// <param name="actionExecutedContext">执行的上下文的操作</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ReWriteResponseContent(actionExecutedContext);
            LoggerExceptionInfo(actionExecutedContext);
        }

        /// <summary>
        /// 重写输出流内容
        /// </summary>
        /// <param name="actionExecutedContext">执行的上下文的操作</param>
        private void ReWriteResponseContent(HttpActionExecutedContext actionExecutedContext)
        {
            var result = new ResultModel<Exception>(actionExecutedContext.Exception);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="actionExecutedContext">执行的上下文的操作</param>
        private void LoggerExceptionInfo(HttpActionExecutedContext actionExecutedContext)
        {
            var actionName = FilterUtils.GetActionFullName(actionExecutedContext);
            var args = FilterUtils.GetRequestArgsJson(actionExecutedContext);

            var info = $"调用接口: {actionName}{Environment.NewLine}调用参数: {args}";
            Logger.Error(info, actionExecutedContext.Exception);
        }
    }
}