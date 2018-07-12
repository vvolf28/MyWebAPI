using MyWebAPI.Models;
using System;
using System.Net;
using System.Net.Http;
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

            var result = ReWriteResponseContent(actionExecutedContext);
            LoggerActionExecInfo(actionExecutedContext, result);
        }

        /// <summary>
        /// 重写输出流内容
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        private ResultModel<object> ReWriteResponseContent(HttpActionExecutedContext actionExecutedContext)
        {
            var data = FilterUtils.GetResponseContent(actionExecutedContext);
            var result = new ResultModel<object>(data);

            var status = actionExecutedContext.ActionContext.Response.StatusCode;
            if (status == HttpStatusCode.NoContent) status = HttpStatusCode.OK;
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(status, result);

            return result;
        }

        /// <summary>
        /// 记录API请求日志
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="result"></param>
        private void LoggerActionExecInfo(HttpActionExecutedContext actionExecutedContext, ResultModel<object> result)
        {
            var actionName = FilterUtils.GetActionFullName(actionExecutedContext);
            var args = FilterUtils.GetRequestArgsJson(actionExecutedContext);
            var info = $"调用接口: {actionName}{Environment.NewLine}调用参数: {args}{Environment.NewLine}返回值: {JsonEx.ToJson(result)}";
            
            Logger.Info(info);
        }
    }
}