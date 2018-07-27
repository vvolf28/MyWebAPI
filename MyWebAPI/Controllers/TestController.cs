using System;
using System.Collections.Generic;
using System.Web.Http;

namespace MyWebAPI.Controllers
{
    /// <summary>
    /// 测试Api控制器
    /// </summary>
    public class TestController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<TestEntity> GetInfoByNoArgs()
        {
            return new List<TestEntity>()
            {
                new TestEntity(){Name = "Name-1", Age = 11, IdCardNo="12345" },
                new TestEntity(){Name = "Name-2", Age = 21, IdCardNo="54321" },
                new TestEntity(){Name = "Name-3", Age = 31, IdCardNo="67890" },
            };
        }


        /// <summary>
        /// GetInfoByArgs
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="idCardNo"></param>
        /// <returns></returns>
        [HttpGet]
        public TestEntity GetInfoByArgs(string name, int age, string idCardNo)
        {
            return new TestEntity() { Name = name, IdCardNo = idCardNo, Age = age };
        }


        /// <summary>
        /// PostInfoByNoArgs
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool PostInfoByNoArgs()
        {
            return true;
        }

        /// <summary>
        /// PostInfoByArgs
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool PostInfoByArgs([FromBody] IList<TestEntity> value)
        {
            if (value == null || value.Count == 0) throw new ArgumentNullException("参数不可为空", nameof(value));

            return value.Count < 3;
        }


        public class TestEntity
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public string IdCardNo { get; set; }
        }
    }
}
