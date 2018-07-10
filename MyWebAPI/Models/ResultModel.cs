using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebAPI.Models
{
    /// <summary>
    /// API返回值模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; set; }


        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }


        /// <summary>
        /// 返回值
        /// </summary>
        public T Data { get; set; }

        
        public ResultModel(T data)
        {
            if(data is Exception)
            {
                this.IsSuccess = false;
                this.Error = (data as Exception).Message;
            }
            else
            {
                this.IsSuccess = true;
                this.Data = data ;
            }
        }
    }
}