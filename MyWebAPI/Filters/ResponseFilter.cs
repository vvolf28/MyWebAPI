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
            ExecFilterBiz(actionExecutedContext);
            base.OnActionExecuted(actionExecutedContext);
        }


        ///// <summary>
        ///// 重写调用后请求(异步)
        ///// </summary>
        ///// <param name="actionExecutedContext"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        //{
        //    ExecFilterBiz(actionExecutedContext);
        //    return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        //}


        private void ExecFilterBiz(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null) return;

            var result = RewriteResponseContent(actionExecutedContext);
            LoggerActionExecInfo(actionExecutedContext, result);
        }


        private ResultModel<object> RewriteResponseContent(HttpActionExecutedContext actionExecutedContext)
        {
            var data = FilterUtils.GetResponseContent(actionExecutedContext);
            var result = new ResultModel<object>(data);
            var status = actionExecutedContext.ActionContext.Response.StatusCode;
            if (status == System.Net.HttpStatusCode.NoContent) status = System.Net.HttpStatusCode.OK;
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(status, result);

            return result;
        }


        private void LoggerActionExecInfo(HttpActionExecutedContext actionExecutedContext, ResultModel<object> result)
        {
            var actionName = FilterUtils.GetActionFullName(actionExecutedContext);
            var args = FilterUtils.GetRequestArgsJson(actionExecutedContext);
            var info = $"调用接口: {actionName}{Environment.NewLine}调用参数: {args}{Environment.NewLine}返回值: {JsonEx.ToJson(result)}";

            Logger.Info(info);
        }

        
    }
}