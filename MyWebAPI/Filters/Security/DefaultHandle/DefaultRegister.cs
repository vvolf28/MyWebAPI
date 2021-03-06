﻿using MyWebAPI.Filters.Security.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyWebAPI.Filters.Security
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
            var registerInfos = GetConfigFromFile();

            m_Cache = registerInfos?.ToDictionary(p => p.AppId);
        }


        private static List<RegisterInfo> GetConfigFromFile()
        {
            var cfgPath = HttpContext.Current.Server.MapPath("~/App_Data\\RegisterInfo.json");
            if (!File.Exists(cfgPath)) throw new FileNotFoundException("配置文件不存在或文件路径错误!", cfgPath);

            return JsonEx.FromJson<List<RegisterInfo>>(File.ReadAllText(cfgPath));
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
                throw new ArgumentException("未注册的接入者!");
            }
        }
    }
}