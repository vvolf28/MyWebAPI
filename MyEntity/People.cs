using System;

namespace MyEntity
{
    /// <summary>
    /// 人员
    /// </summary>
    public class People
    {
        /// <summary>
        /// 对象ID
        /// </summary>
        public string ObjId { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 性别
        /// </summary>
        public EnumSex Sex { get; set; }


        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        
    }

    /// <summary>
    /// 性别类型
    /// </summary>
    public enum EnumSex
    {
        /// <summary>
        /// 女
        /// </summary>
        Fmale = 0,

        /// <summary>
        /// 男
        /// </summary>
        Male = 1
    }
}
