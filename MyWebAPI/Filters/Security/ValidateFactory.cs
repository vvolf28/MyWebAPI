using MyWebAPI.Filters.Security.DefaultHandle;
using MyWebAPI.Filters.Security.Interface;

namespace MyWebAPI.Filters.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public static ICallFrequency CallFrequencyInstance { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static IIpAddress IpAddressInstance { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static IRegister RegisterInstance { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static ISecurity SecurityInstance { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static ITimeStamp TimeStampInstance { get; private set; }


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