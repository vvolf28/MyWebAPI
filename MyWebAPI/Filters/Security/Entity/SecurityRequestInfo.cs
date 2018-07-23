namespace MyWebAPI.Filters.Security.Entity
{
    public class SecurityRequestInfo
    {
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