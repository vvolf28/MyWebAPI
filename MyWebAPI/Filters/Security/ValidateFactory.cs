using MyWebAPI.Filters.Security.DefaultHandle;
using MyWebAPI.Filters.Security.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebAPI.Filters.Security
{
    public class ValidateFactory
    {
        public static ICallFrequency CallFrequencyInstance { get; private set; }

        public static IIpAddress IpAddressInstance { get; private set; }

        public static IRegister RegisterInstance { get; private set; }

        public static ISecurity SecurityInstance { get; private set; }

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