using MyWebAPI.Filters.Security.Interface;
using System;

namespace MyWebAPI.Filters.Security.DefaultHandle
{
    public class DefaultTimeStamp : ITimeStamp
    {
        private static int m_OverTimeForMinute = 5;
        
        public void Validate(string timeStamp)
        {
            if (string.IsNullOrWhiteSpace(timeStamp)) throw new ArgumentNullException("时间戳数据为空!", nameof(timeStamp));
            if (!DateTime.TryParse(timeStamp, out DateTime requestTime)) throw new ArgumentException("时间戳格式不正确!", nameof(timeStamp));

            if (Math.Abs(DateTime.Now.Subtract(requestTime).TotalMinutes) > m_OverTimeForMinute) throw new Exception("请求时间戳已超时!!!");
        }
    }
}