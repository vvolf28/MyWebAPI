using MyWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MyWebAPI.Filters
{
    /// <summary>
    /// 日志处理
    /// </summary>
    public class LogInfoHandle
    {
        public static string CreateRequestLogInfo(HttpActionContext actionContext)
        {
            var args = JsonEx.ToJson(actionContext.ActionArguments);
            var info = $"接口: {FilterUtils.GetActionFullName(actionContext)}{Environment.NewLine}输入参数:{args}";
            return info;
        }

        public static string CreateResponseLogInfo(HttpActionExecutedContext actionExecutedContext)
        {
            //TODO:空返回值处理
            string apiName = FilterUtils.GetActionFullName(actionExecutedContext.ActionContext);
            string args = JsonEx.ToJson(actionExecutedContext.ActionContext.ActionArguments);

            var data = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result;
            var result = new ResultModel<object>(data);
            var info = $"接口: {FilterUtils.GetActionFullName(actionExecutedContext.ActionContext)}{Environment.NewLine}返回值: {JsonEx.ToJson(result)}";
            return info;
        }
    }
}