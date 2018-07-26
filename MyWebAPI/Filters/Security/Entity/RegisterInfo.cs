using System.Collections.Generic;

namespace MyWebAPI.Filters.Security.Entity
{
    /// <summary>
    /// 应用注册信息
    /// </summary>
    public class RegisterInfo
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string AppSecret { get; set; }


        /// <summary>
        /// 授权访问Ip列表
        /// </summary>
        public List<string> AccessIpList { get; set; }
    }
}