using MyWebAPI.Filters.Security.Entity;
using MyWebAPI.Filters.Security.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

namespace MyWebAPI.Filters.Security.DefaultHandle
{
    public class DefaultSecurity : ISecurity
    {
        private static readonly Encoding m_Encoding = Encoding.UTF8;

        public SecurityRequestInfo GetSecurityInfo(HttpActionContext context)
        {
            var result = new SecurityRequestInfo
            {
                AppId = GetHeaderVaule(context.Request.Headers, "appId"),
                TimeStamp = GetHeaderVaule(context.Request.Headers, "timestamp"),
                Signature = GetHeaderVaule(context.Request.Headers, "signature"),
                RequestContent = GetRequestData(context)
            };

            return result;
        }


        public void Validate(SecurityRequestInfo securityInfo, RegisterInfo registerInfo)
        {
            this.ValidateHttpHeader(securityInfo);
            var content = $"{securityInfo.AppId}{securityInfo.TimeStamp}{securityInfo.RequestContent}{registerInfo.AppSecret}";
            var sign = GetMD5(content);
            if (sign != securityInfo.Signature) throw new Exception("签名验证错误!");
        }


        private void ValidateHttpHeader(SecurityRequestInfo securityInfo)
        {
            if (string.IsNullOrWhiteSpace(securityInfo.AppId)) throw new ArgumentNullException("AppId不可为空!", nameof(securityInfo.AppId));
            if (string.IsNullOrWhiteSpace(securityInfo.Signature)) throw new ArgumentNullException("签名不可为空!", nameof(securityInfo.Signature));
            if (string.IsNullOrWhiteSpace(securityInfo.TimeStamp)) throw new ArgumentNullException("时间戳不可为空!", nameof(securityInfo.TimeStamp));
        }


        private string GetHeaderVaule(HttpRequestHeaders headers, string name)
        {
            if (headers == null) return string.Empty;

            var isExists = headers.TryGetValues(name, out IEnumerable<string> values);
            if (!isExists) return string.Empty;

            var tmp = values.ToList();
            return tmp.Count == 1 ? tmp[0] : string.Empty;
        }


        #region 【获取请求数据】
        private string GetRequestData(HttpActionContext context)
        {
            if (context.Request.Method == HttpMethod.Post) return this.GetPostData(context);
            if (context.Request.Method == HttpMethod.Get) return this.GetUrlData(context);

            throw new Exception("请求类型错误!");
        }


        private string GetPostData(HttpActionContext context)
        {
            return context.Request.Content.ReadAsStringAsync().Result;
        }


        private string GetUrlData(HttpActionContext context)
        {
            var request = ((HttpContextWrapper)context.Request.Properties["MS_HttpContext"]).Request;
            return request.QueryString.ToString();
        }

        #endregion


        #region 【MD5字符串】
        /// <summary>
        /// 获取MD5值
        /// </summary>
        /// <param name="data">字符串数据</param>
        /// <returns>MD5值</returns>
        private static string GetMD5(string data)
        {
            var md5 = MD5.Create();
            var buffer = md5.ComputeHash(m_Encoding.GetBytes(data));
            return GetHexString(buffer);
        }


        /// <summary>
        /// 获取16进制小写格式字符串
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <returns>16进制的小写格式字符串</returns>
        private static string GetHexString(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", "").ToLower();
        }

        #endregion
    }
}