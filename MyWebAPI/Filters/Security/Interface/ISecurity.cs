using MyWebAPI.Filters.Security.Entity;
using System.Web.Http.Controllers;

namespace MyWebAPI.Filters.Security.Interface
{
    public interface ISecurity
    {
        SecurityRequestInfo GetSecurityInfo(HttpActionContext context);

        void Validate(SecurityRequestInfo requestCertificationInfo, RegisterInfo registerInfo);
    }
}
