using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MyWebAPI.Filters
{
    /// <summary>
    /// Api安全校验过滤器
    /// </summary>
    public class ApiSecurityFilter : ActionFilterAttribute
    {
        private static readonly Dictionary<string, ThirdRegisterInfo> m_InfoCache;

        #region 【初始化数据】
        static ApiSecurityFilter()
        {
            m_InfoCache = LoadThirdRegisterInfo();
        }
        
        private static Dictionary<string, ThirdRegisterInfo> LoadThirdRegisterInfo()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data\\SecurityConfig.json");
            var jsonData = JsonEx.FromJson<List<ThirdRegisterInfo>>(File.ReadAllText(path));
            return jsonData == null ?
                new Dictionary<string, ThirdRegisterInfo>() :
                jsonData.ToDictionary(p => p.ApiKey);
        }

        #endregion


        /// <summary>
        /// Api接口调用时处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(HttpActionContext context)
        {
            var paramStr = GetRequestData(context);
            var securityInfo = GetSecurityInfo(context);
            this.ValidateHttpHeader(securityInfo);

            var thirdInfo = GetThirdInfo(securityInfo);

            //this.ValidateIpAddress(thirdInfo);
            this.ValidateInterfaceCallFrequency(thirdInfo);
            this.ValidateTimeStamp(securityInfo.TimeStamp);
            this.ValidateSing(securityInfo, thirdInfo, paramStr);
        }


        #region 【获取安全认证信息】
        /// <summary>
        /// 从请求上下文中获取安全认证信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private RequestSecurityInfo GetSecurityInfo(HttpActionContext context)
        {
            var result = new RequestSecurityInfo();

            result.ApiKey = GetHeaderVaule(context.Request.Headers, "apiKey");
            result.TimeStamp = GetHeaderVaule(context.Request.Headers, "timestamp");
            result.Sign = GetHeaderVaule(context.Request.Headers, "sign");

            return result;
        }


        /// <summary>
        /// 从请求头中获取数据
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetHeaderVaule(HttpRequestHeaders headers, string name)
        {
            if (headers == null) return string.Empty;

            IEnumerable<string> values;
            var isExists = headers.TryGetValues(name, out values);
            if (!isExists) return string.Empty;

            var tmp = values.ToList();
            return tmp.Count == 1 ? tmp[0] : string.Empty;
        }

        #endregion


        #region 【获取请求数据】
        private string GetRequestData(HttpActionContext context)
        {
            if (context.Request.Method == HttpMethod.Post) return this.GetPostData(context);
            if (context.Request.Method == HttpMethod.Get) return this.GetUrlData(context);

            throw new Exception("请求类型错误!");
        }


        private string GetPostData(HttpActionContext context)
        {
            var stream = context.Request.Content.ReadAsStreamAsync().Result;
            return context.Request.Content.ReadAsStringAsync().Result;

            var requestDataStr = "";
            if (stream != null && stream.Length > 0)
            {
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    requestDataStr = reader.ReadToEnd().ToString();
                    stream.Position = 0;
                }
            }
            return requestDataStr;
        }


        private string GetUrlData(HttpActionContext context)
        {
            var request = ((HttpContextWrapper)context.Request.Properties["MS_HttpContext"]).Request;
            return request.QueryString.ToString();
        }

        #endregion


        #region 【获取第三方接入信息】
        private ThirdRegisterInfo GetThirdInfo(RequestSecurityInfo reqInfo)
        {
            if (!m_InfoCache.ContainsKey(reqInfo.ApiKey)) throw new Exception("未注册的第三方接入者!");
            return m_InfoCache[reqInfo.ApiKey];
        }

        #endregion


        #region 【HttpHeader 校验】
        private void ValidateHttpHeader(RequestSecurityInfo securityInfo)
        {
            if (string.IsNullOrWhiteSpace(securityInfo.ApiKey)) throw new Exception("apiKey不可为空!");
            if (string.IsNullOrWhiteSpace(securityInfo.Sign)) throw new Exception("签名不可为空!");
            if (string.IsNullOrWhiteSpace(securityInfo.TimeStamp)) throw new Exception("时间戳不可为空!");
        }

        #endregion
        

        #region 【Ip地址校验】
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        private void ValidateIpAddress(ThirdRegisterInfo info)
        {
            var ip = GetRequestIpAddress();
            if (!info.IpAddressList.Contains(ip)) throw new Exception("非法Ip地址访问!");
        }

        private string GetRequestIpAddress()
        {
            var result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; 
            if (string.IsNullOrWhiteSpace(result)) result = HttpContext.Current.Request.ServerVariables["HTTP_CDN_SRC_IP"];
            if (string.IsNullOrWhiteSpace(result)) result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrWhiteSpace(result) || !IsIpAddress(result)) return "127.0.0.1";

            return result;
        }

        private bool IsIpAddress(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion


        #region 【调用频次校验】
        private void ValidateInterfaceCallFrequency(ThirdRegisterInfo info)
        {
            //TODO: 调用频次校验， 暂不限制
        }


        #endregion


        #region 【时间戳校验】
        /// <summary>
        /// /校验请求时间戳是否合法
        /// </summary>
        /// <param name="timeStamp"></param>
        private void ValidateTimeStamp(string timeStamp)
        {
            if (string.IsNullOrWhiteSpace(timeStamp)) throw new Exception("时间戳数据为空!");
            DateTime requestTime;
            if (!DateTime.TryParse(timeStamp, out requestTime)) throw new Exception("时间戳格式不正确!");

            if (Math.Abs(DateTime.Now.Subtract(requestTime).TotalMinutes) > 5) throw new Exception("时间戳非法!");

            //if ((DateTime.Now - requestTime) > TimeSpan.FromMinutes(5)) throw new Exception("时间戳超时！");
        }

        #endregion
        

        #region 【内容签名校验】
        private void ValidateSing(RequestSecurityInfo securityInfo, ThirdRegisterInfo info, string paramStr)
        {
            var content = $"{securityInfo.ApiKey}{securityInfo.TimeStamp}{paramStr}{info.ApiSecret}";
            var sign = BuildMD5Sign(content);
            if (sign != securityInfo.Sign) throw new Exception("签名不正确!");
        }


        private string BuildMD5Sign(string content)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(content));

            var result = new StringBuilder();
            foreach(var item in hash)
            {
                result.Append(item.ToString("x2"));
            }

            return result.ToString().ToUpper();
        }

        #endregion
        
    }

    /// <summary>
    /// 安全认证信息实体
    /// </summary>
    class RequestSecurityInfo
    {
        /// <summary>
        /// 调用者身份
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
    }


    public class ThirdRegisterInfo
    {
        public string Name { get; set; }


        public string Account { get; set; }


        public string ApiKey { get; set; }


        public string ApiSecret { get; set; }


        public List<string> IpAddressList { get; set; }
    }
}