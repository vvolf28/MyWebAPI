using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MyWebAPI.Filters.Security
{
    /// <summary>
    /// Api安全校验过滤器
    /// </summary>
    public class ApiSecurityFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Api接口调用时处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(HttpActionContext context)
        {
            var requetInfo = ValidateFactory.SecurityInstance.GetSecurityInfo(context);
            var registerInfo = ValidateFactory.RegisterInstance.GetRegister(requetInfo.AppId);

            ValidateFactory.TimeStampInstance.Validate(requetInfo.TimeStamp);
            ValidateFactory.IpAddressInstance.Validate(registerInfo.AccessIpList);
            ValidateFactory.CallFrequencyInstance.Validate(requetInfo.AppId);
            ValidateFactory.SecurityInstance.Validate(requetInfo, registerInfo);
        }
    }
}