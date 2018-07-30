using System.Collections.Generic;

namespace MyWebAPI.Filters.Security.Interface
{
    /// <summary>
    /// 授权Ip地址接口
    /// </summary>
    public interface IIPAddress
    {
        /// <summary>
        /// 验证Ip是否合法
        /// </summary>
        /// <param name="accessIPList">授权Ip地址列表</param>
        void Validate(IList<string> accessIPList);
    }
}
