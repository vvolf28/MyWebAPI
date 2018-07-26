namespace MyWebAPI.Filters.Security.Interface
{
    /// <summary>
    /// 调用频率接口
    /// </summary>
    public interface ICallFrequency
    {
        /// <summary>
        /// 验证调用频率
        /// </summary>
        /// <param name="appId">应用Id</param>
        void Validate(string appId);
    }
}
