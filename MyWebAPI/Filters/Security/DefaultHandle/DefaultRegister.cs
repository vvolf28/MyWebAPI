using MyWebAPI.Filters.Security.Entity;
using MyWebAPI.Filters.Security.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyWebAPI.Filters.Security.DefaultHandle
{
    /// <summary>
    /// 默认注册信息操作
    /// </summary>
    public class DefaultRegister : IRegister
    {
        /// <summary>
        /// 注册信息缓存
        /// </summary>
        private Dictionary<string, RegisterInfo> m_Cache;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultRegister()
        {
            var cfgPath = HttpContext.Current.Server.MapPath("~/App_Data\\RegisterInfo.json");
            if (!File.Exists(cfgPath)) throw new Exception("配置文件不存在或文件路径错误!");

            var registerInfos = JsonEx.FromJson<List<RegisterInfo>>(File.ReadAllText(cfgPath));

            if (registerInfos == null)
            {
                m_Cache = new Dictionary<string, RegisterInfo>();
            }
            else
            {
                m_Cache = registerInfos.ToDictionary(p => p.AppId);
            }
        }

        /// <summary>
        /// 获取注册信息
        /// </summary>
        /// <param name="appId">注册Id</param>
        /// <returns>注册信息实体</returns>
        public RegisterInfo GetRegister(string appId)
        {
            var isGet = m_Cache.TryGetValue(appId, out RegisterInfo data);

            if (isGet)
            {
                return data;
            }
            else
            {
                throw new Exception("未注册的接入者!");
            }
        }
    }
}