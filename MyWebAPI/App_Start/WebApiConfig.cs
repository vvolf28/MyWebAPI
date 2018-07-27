using MyWebAPI.Filters;
using MyWebAPI.Filters.Security;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MyWebAPI
{
    /// <summary>
    /// api配置
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Web API 配置和服务注册
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "API",
                routeTemplate: "api/{controller}/{action}"
                //defaults: new
                //{
                //    id = RouteParameter.Optional
                //}
            );


            #region 注册全局过滤器
            config.Filters.Add(new ApiSecurityFilter());
            //RequestFilter暂不使用。出入参数统一由ResponseFilter进行处理
            //config.Filters.Add(new RequestFilter()); 
            config.Filters.Add(new ExceptionFilter());
            config.Filters.Add(new ResponseFilter());
            #endregion


            #region【Json序列化设置(驼峰命名)】

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            #endregion
        }
    }
}
