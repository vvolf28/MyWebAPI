using MyWebAPI.Filters.Security.DefaultHandle;
using MyWebAPI.Filters.Security.Interface;

namespace MyWebAPI.Filters.Security
{
    /// <summary>
    /// 安全认证实例工厂
    /// </summary>
    public class ValidateFactory
    {
        /// <summary>
        /// 调用频率接口实例
        /// </summary>
        public static ICallFrequency CallFrequencyInstance { get; private set; }

        /// <summary>
        /// 授权Ip地址接口实例
        /// </summary>
        public static IIpAddress IpAddressInstance { get; private set; }

        /// <summary>
        /// 注册信息接口实例
        /// </summary>
        public static IRegister RegisterInstance { get; private set; }

        /// <summary>
        /// 安全认证接口实例
        /// </summary>
        public static ISecurity SecurityInstance { get; private set; }

        /// <summary>
        /// 时间戳验证接口实例
        /// </summary>
        public static ITimeStamp TimeStampInstance { get; private set; }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ValidateFactory()
        {
            CallFrequencyInstance = new DefaultCallFrequency();
            IpAddressInstance = new DefaultIpAddress();
            RegisterInstance = new DefaultRegister();
            SecurityInstance = new DefaultSecurity();
            TimeStampInstance = new DefaultTimeStamp();
        }
    }
}