﻿using MyWebAPI.Models;
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
        public static string GetRequestArgsJson(HttpActionExecutedContext actionExecutedContext)
        {
            return GetRequestArgsJson(actionExecutedContext.ActionContext);
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static string GetRequestArgsJson(HttpActionContext actionContext)
        {
            return JsonEx.ToJson(actionContext.ActionArguments);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public static object GetResponseContent(HttpActionExecutedContext actionExecutedContext)
        {
            return GetResponseContent(actionExecutedContext.ActionContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static object GetResponseContent(HttpActionContext actionContext)
        {
            return actionContext.Response.Content?.ReadAsAsync<object>().Result;
        }
    }
}