using System.Web.Http.Controllers;

namespace MyWebAPI.Filters.Security.Interface
{
    /// <summary>
    /// 安全认证接口
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        /// 获取安全认证信息
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns>安全认证信息实体</returns>
        SecurityRequestInfo GetSecurityInfo(HttpActionContext context);


        /// <summary>
        /// 验证安全信息是否合法
        /// </summary>
        /// <param name="securityInfo">安全信息实体</param>
        /// <param name="registerInfo">注册信息实体</param>
        void Validate(SecurityRequestInfo securityInfo, RegisterInfo registerInfo);
    }
}
