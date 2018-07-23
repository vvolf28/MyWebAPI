using MyWebAPI.Filters.Security.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MyWebAPI.Filters.Security.DefaultHandle
{
    public class DefaultIpAddress : IIpAddress
    {
        public void Validate(List<string> accessIpList)
        {
            var ip = GetRequestIpAddress();
            if(accessIpList == null || accessIpList.Count == 0)
            {
                return;
            }
            else
            {
                if (!accessIpList.Contains(ip)) throw new Exception("非法Ip地址访问!");
            }
            
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
    }
}