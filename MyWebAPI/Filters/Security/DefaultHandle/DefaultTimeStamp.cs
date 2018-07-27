using MyWebAPI.Filters.Security.Interface;
using System;

namespace MyWebAPI.Filters.Security.DefaultHandle
{
    /// <summary>
    /// 默认时间戳验证
    /// </summary>
    public class DefaultTimeStamp : ITimeStamp
    {
        /// <summary>
        /// 超时时间(分钟)
        /// </summary>
        private static readonly int m_OverTimeForMinute = 1;
        
        /// <summary>
        /// 验证时间戳是否超时
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        public void Validate(string timeStamp)
        {
            if (string.IsNullOrWhiteSpace(timeStamp)) throw new ArgumentNullException("时间戳数据为空!", nameof(timeStamp));
            if (!DateTime.TryParse(timeStamp, out DateTime requestTime)) throw new ArgumentException("时间戳格式不正确!", nameof(timeStamp));

            if (Math.Abs(DateTime.Now.Subtract(requestTime).TotalMinutes) > m_OverTimeForMinute) throw new Exception("请求时间戳已超时!!!");
        }
    }
}