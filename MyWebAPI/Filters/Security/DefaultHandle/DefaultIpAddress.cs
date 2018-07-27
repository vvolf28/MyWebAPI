using MyWebAPI.Filters.Security.Interface;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace MyWebAPI.Filters.Security.DefaultHandle
{
    /// <summary>
    /// 默认授权Ip地址验证
    /// </summary>
    public class DefaultIpAddress : IIpAddress
    {
        /// <summary>
        /// 获取Ip方法列表
        /// </summary>
        private IList<Func<string>> m_HandleList;


        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultIpAddress()
        {
            m_HandleList = new List<Func<string>>()
            {
                GetClientIpWithProxy,
                GetClientIpWithoutProxy,
                GetClientIpByUserHost,
                GetClientIpByDefault,
            };
        }


        /// <summary>
        /// 验证Ip是否合法
        /// </summary>
        /// <param name="accessIpList">授权Ip地址列表</param>
        public void Validate(List<string> accessIpList)
        {
            //访问控制列表为空时，默认为不做ip校验
            if (accessIpList == null || accessIpList.Count == 0) return;

            var ip = GetRealRequestIp();
            if (!accessIpList.Contains(ip)) throw new Exception("非法Ip地址访问!");
        }


        /// <summary>
        /// 获取真实请求来源的Ip地址
        /// </summary>
        /// <returns>请求者的Ip地址字符串格式</returns>
        private string GetRealRequestIp()
        {
            var clientIp = string.Empty;

            foreach(var handle in m_HandleList)
            {
                clientIp = GetClientIpHandle(handle);
                if (!string.IsNullOrWhiteSpace(clientIp)) break;
            }

            return clientIp;
        }


        /// <summary>
        /// 获取Ip地址代理
        /// </summary>
        /// <param name="getHandle"></param>
        /// <returns></returns>
        private string GetClientIpHandle(Func<string> getHandle)
        {
            var clientIp = getHandle();
            if (IsIpAddress(clientIp))
            {
                return clientIp;
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 通过代理获取客户端Ip
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 使用代理服务器: 
        /// 非安全获取，客户端Ip可被伪造
        ///     1. 使用透明代理服务器的情况：Transparent Proxies
        ///        REMOTE_ADDR: 最后一个代理服务器 IP 
        ///        HTTP_VIA: 代理服务器 IP
        ///        X-Forwarded-For:真实Ip列表， 格式如:client1, proxy1, proxy2
        ///     2. 使用普通匿名代理服务器的情况：Anonymous Proxies
        ///        REMOTE_ADDR = 最后一个代理服务器 IP 
        ///        HTTP_VIA = 代理服务器 IP
        ///        HTTP_X_FORWARDED_FOR = 代理服务器 IP列表,隐藏了客户的真实IP，但是向访问对象透露了是使用代理服务器访问的。
        ///     3. 使用欺骗性代理服务器的情况：Distorting Proxies
        ///        REMOTE_ADDR = 代理服务器 IP 
        ///        HTTP_VIA = 代理服务器 IP
        ///        HTTP_X_FORWARDED_FOR = 随机的IP列表,告诉了访问对象使用了代理服务器，但编造了一个虚假的随机IP代替客户的真实IP欺骗代理服务器。
        ///     4. 使用高匿名代理服务器的情况：High Anonymity Proxies (Elite proxies)
        ///        REMOTE_ADDR = 代理服务器 IP 
        ///        HTTP_VIA = 没数值或不显示
        ///        HTTP_X_FORWARDED_FOR = 没数值或不显示(完全用代理服务器的信息替代了客户端的所有信息，就象就是完全使用那台代理服务器直接访问对象)
        /// </remarks>
        private string GetClientIpWithProxy()
        {
            if (string.IsNullOrWhiteSpace(GetRequestServerVariable(HttpHeadString.VIA))) return string.Empty;
            var ipListStr = GetRequestServerVariable(HttpHeadString.X_FORWARDED_FOR);
            if (string.IsNullOrWhiteSpace(ipListStr)) return string.Empty;

            var ipList = ipListStr.Split(',');
            if (ipList.Length < 1) return string.Empty;

            return ipList[0].Trim();
        }


        /// <summary>
        /// 没有使用代理情况下获取客户端IP
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 没有使用代理服务器
        ///     REMOTE_ADDR = 客户端IP
        ///     HTTP_VIA = 没数值或不显示
        ///     HTTP_X_FORWARDED_FOR = 没数值或不显示
        /// </remarks>
        private string GetClientIpWithoutProxy()
        {
           return GetRequestServerVariable(HttpHeadString.REMOTE_ADDR);
        }


        /// <summary>
        /// 使用CDN后获取客户IP
        /// </summary>
        /// <returns></returns>
        private string GetClientIpByCNDSRC()
        {
            return GetRequestServerVariable(HttpHeadString.CDN_SRC_IP);
        }
        

        /// <summary>
        /// 从UserHostAddress获取客户IP
        /// </summary>
        /// <returns></returns>
        private string GetClientIpByUserHost()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }


        /// <summary>
        /// 获取默认Ip地址
        /// </summary>
        /// <returns></returns>
        private string GetClientIpByDefault()
        {
            return "127.0.0.1";
        }


        /// <summary>
        /// 从Http请求中获取指定环境变量的值
        /// </summary>
        /// <param name="varName">环境变量名称</param>
        /// <returns></returns>
        private string GetRequestServerVariable(string varName)
        {
            return HttpContext.Current.Request.ServerVariables[varName];
        }


        /// <summary>
        /// 判断是否为合法Ip地址格式
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>是否是Ip地址</returns>
        private bool IsIpAddress(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }


        /// <summary>
        /// Http头字符串定义
        /// </summary>
        class HttpHeadString
        {
            public static readonly string VIA = "HTTP_VIA";
            public static readonly string X_FORWARDED_FOR = "HTTP_X_FORWARDED_FOR";
            public static readonly string REMOTE_ADDR = "REMOTE_ADDR";
            public static readonly string CDN_SRC_IP = "HTTP_CDN_SRC_IP";
        }
    }
}