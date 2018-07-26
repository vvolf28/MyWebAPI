namespace MyWebAPI.Filters.Security.Interface
{
    /// <summary>
    /// 时间戳验证接口
    /// </summary>
    public interface ITimeStamp
    {
        /// <summary>
        /// 验证时间戳是否超时
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        void Validate(string timeStamp);
    }
}
