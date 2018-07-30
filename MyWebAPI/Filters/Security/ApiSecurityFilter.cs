using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MyWebAPI.Filters.Security
{
    /// <summary>
    /// Api安全校验拦截器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ApiSecurityFilter : ActionFilterAttribute
    {
        /// <summary>
        /// WebApi 安全认证拦截(在调用操作方法之前发生)
        /// </summary>
        /// <param name="actionContext">操作上下文</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var requetInfo = ValidateFactory.SecurityInstance.GetSecurityInfo(actionContext);
            var registerInfo = ValidateFactory.RegisterInstance.GetRegister(requetInfo.AppId);

            ValidateFactory.TimeStampInstance.Validate(requetInfo.TimeStamp);
            ValidateFactory.IPAddressInstance.Validate(registerInfo.AccessIPList);
            ValidateFactory.CallFrequencyInstance.Validate(requetInfo.AppId);
            ValidateFactory.SecurityInstance.Validate(requetInfo, registerInfo);
        }
    }
}