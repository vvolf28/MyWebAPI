using MyWebAPI.Filters.Security.Interface;

namespace MyWebAPI.Filters.Security
{
    /// <summary>
    /// 默认调用频率验证
    /// </summary>
    public class DefaultCallFrequency : ICallFrequency
    {
        /// <summary>
        /// 验证调用频率
        /// </summary>
        /// <param name="appId">应用Id</param>
        public void Validate(string appId)
        {
            //TODO: 暂不实现
            return;
        }
    }
}