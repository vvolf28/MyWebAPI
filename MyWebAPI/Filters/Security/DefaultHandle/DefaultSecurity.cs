using MyWebAPI.Filters.Security.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

namespace MyWebAPI.Filters.Security
{
    /// <summary>
    /// 默认安全认证
    /// </summary>
    public class DefaultSecurity : ISecurity
    {
        /// <summary>
        /// 字符集
        /// </summary>
        private static readonly Encoding s_Encoding = Encoding.UTF8;

        /// <summary>
        /// 获取安全认证信息
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns>安全认证信息实体</returns>
        public SecurityRequestInfo GetSecurityInfo(HttpActionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context), "请求上下文不可为空!");

            var result = new SecurityRequestInfo
            {
                AppId = GetHeaderVaule(context.Request.Headers, "appId"),
                TimeStamp = GetHeaderVaule(context.Request.Headers, "timestamp"),
                Signature = GetHeaderVaule(context.Request.Headers, "signature"),
                RequestContent = GetRequestData(context)
            };

            return result;
        }

        /// <summary>
        /// 验证安全信息是否合法
        /// </summary>
        /// <param name="securityInfo">安全信息实体</param>
        /// <param name="registerInfo">注册信息实体</param>
        public void Validate(SecurityRequestInfo securityInfo, RegisterInfo registerInfo)
        {
            if (securityInfo == null) throw new ArgumentNullException(nameof(securityInfo), "安全信息实体不可为空!");
            if (registerInfo == null) throw new ArgumentNullException(nameof(registerInfo), "注册信息实体不可为空!");

            ValidateSecurityInfo(securityInfo);
            var sign = CreatSingData(securityInfo, registerInfo);
            if (sign != securityInfo.Signature) throw new ArgumentException("签名验证错误!");
        }


        /// <summary>
        /// 根据请求内容及注册信息，生成签名数据
        /// </summary>
        /// <param name="securityInfo">安全信息实体</param>
        /// <param name="registerInfo">注册信息实体</param>
        /// <returns></returns>
        private static string CreatSingData(SecurityRequestInfo securityInfo, RegisterInfo registerInfo)
        {
            var content = $"{securityInfo.AppId}{securityInfo.TimeStamp}{securityInfo.RequestContent}{registerInfo.AppSecret}";
            return GetMD5(content);
        }


        /// <summary>
        /// 验证安全信息数据完整性
        /// </summary>
        /// <param name="securityInfo">安全信息实体</param>
        private static void ValidateSecurityInfo(SecurityRequestInfo securityInfo)
        {
            if (string.IsNullOrWhiteSpace(securityInfo.AppId)) throw new ArgumentException("AppId不可为空!");
            if (string.IsNullOrWhiteSpace(securityInfo.Signature)) throw new ArgumentException("签名不可为空!");
            if (string.IsNullOrWhiteSpace(securityInfo.TimeStamp)) throw new ArgumentException("时间戳不可为空!");
        }

        /// <summary>
        /// 根据名称，从请求头中获取对应数据
        /// </summary>
        /// <param name="headers">请求头</param>
        /// <param name="name">名称</param>
        /// <returns>请求头中名称对应的值</returns>
        private static string GetHeaderVaule(HttpRequestHeaders headers, string name)
        {
            if (headers == null) return string.Empty;

            var isExists = headers.TryGetValues(name, out IEnumerable<string> values);

            if (!isExists) return string.Empty;
            if (values.Count() > 1) throw new ArgumentException("HttpHead对应名称存在多个值!", name);

            return values?.First();
        }


        #region 【获取请求数据】
        /// <summary>
        /// 获取请求数据
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns>数据内容</returns>
        private static string GetRequestData(HttpActionContext context)
        {
            if (context.Request.Method == HttpMethod.Post) return GetPostData(context);
            if (context.Request.Method == HttpMethod.Get) return GetUrlData(context);

            throw new HttpRequestException("请求类型错误!");
        }

        /// <summary>
        /// 获取Post请求数据
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns>Post请求数据,字符串格式</returns>
        private static string GetPostData(HttpActionContext context)
        {
            return context.Request.Content?.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 获取Get数据(Url)
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns>Get请求数据，字符串格式</returns>
        private static string GetUrlData(HttpActionContext context)
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
            using (var md5 = MD5.Create())
            {
                var buffer = md5.ComputeHash(s_Encoding.GetBytes(data));
                return GetHexString(buffer);
            }
        }


        /// <summary>
        /// 获取16进制小写格式字符串
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <returns>16进制的小写格式字符串</returns>
        private static string GetHexString(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", "").ToLower(CultureInfo.CurrentCulture);
        }

        #endregion
    }
}