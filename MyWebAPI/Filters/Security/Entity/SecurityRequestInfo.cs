namespace MyWebAPI.Filters.Security
{
    /// <summary>
    /// 请求安全信息
    /// </summary>
    public class SecurityRequestInfo
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        /// <remarks>Guid格式</remarks>
        public string AppId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 请求内容
        /// </summary>
        public string RequestContent { get; set; }

    }
}