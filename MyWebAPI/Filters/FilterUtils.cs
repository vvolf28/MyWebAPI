using MyWebAPI.Models;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MyWebAPI.Filters
{
    /// <summary>
    /// 自定义过滤器工具类
    /// </summary>
    public class FilterUtils
    {
        /// <summary>
        /// 获取请求名称
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public static string GetActionFullName(HttpActionExecutedContext actionExecutedContext)
        {
            return GetActionFullName(actionExecutedContext.ActionContext);
        }

        /// <summary>
        /// 获取请求名称
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static string GetActionFullName(HttpActionContext actionContext)
        {
            return $"{actionContext.ActionDescriptor.ControllerDescriptor.ControllerName}/{actionContext.ActionDescriptor.ActionName}";
        }


        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public static string GetRequestArgs(HttpActionExecutedContext actionExecutedContext)
        {
            return GetRequestArgs(actionExecutedContext.ActionContext);
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static string GetRequestArgs(HttpActionContext actionContext)
        {
            return JsonEx.ToJson(actionContext.ActionArguments);
        }
        

        public static string GetResponseData(HttpActionExecutedContext actionExecutedContext)
        {
            return GetResponseData(actionExecutedContext.ActionContext);
        }


        public static string GetResponseData(HttpActionContext actionContext)
        {
            object data = null;
            if (actionContext.Response.Content != null)
            {
                data = actionContext.Response.Content.ReadAsAsync<object>().Result;
            }

            var result = new ResultModel<object>(data);
            return JsonEx.ToJson(result);
        }
    }
}