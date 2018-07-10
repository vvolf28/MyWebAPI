using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebAPI.Controllers
{
    public class TestController : ApiController
    {
        /// <summary>
        /// 测试GetUser
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetUser()
        {
            return new string[] { "value1", "value2" };
        }


        /// <summary>
        /// 测试Post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void PostUser([FromBody]string value)
        {
        }


        /// <summary>
        /// 新增空用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void AddEmptyUser([FromBody]string msg)
        {
            var id = Guid.NewGuid().ToString();
        }


        [HttpPost]
        public string AddTest([FromBody] int number)
        {
            if(number >= 0)
            {
                return $"Success: number is {number}";
            }
            else
            {
                throw new Exception($"error: number is {number}");
            }
                
        }

        /// <summary>
        /// 测试Delete
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public void DeleteUser([FromBody]int id)
        {
            throw new Exception("id is error");
        }
    }
}
