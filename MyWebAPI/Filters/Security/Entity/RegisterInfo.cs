using System.Collections.Generic;

namespace MyWebAPI.Filters.Security.Entity
{
    public class RegisterInfo
    {
        /// <summary>
        /// 注册Id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string AppSecret { get; set; }


        /// <summary>
        /// 授权Ip列表
        /// </summary>
        public List<string> AccessIpList { get; set; }
    }
}