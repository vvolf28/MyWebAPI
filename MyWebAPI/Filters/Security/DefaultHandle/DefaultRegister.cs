using MyWebAPI.Filters.Security.Entity;
using MyWebAPI.Filters.Security.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyWebAPI.Filters.Security.DefaultHandle
{
    public class DefaultRegister : IRegister
    {
        private Dictionary<string, RegisterInfo> m_Cache;

        public DefaultRegister()
        {
            var cfgPath = HttpContext.Current.Server.MapPath("~/App_Data\\RegisterInfo.json");
            if (!File.Exists(cfgPath)) throw new Exception("配置文件不存在或路径错误!");

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