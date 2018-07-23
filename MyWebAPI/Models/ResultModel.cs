using System;

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

        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public ResultModel(T data)
        {
            if(data is Exception)
            {
                IsSuccess = false;
                Error = (data as Exception)?.Message;
            }
            else
            {
                IsSuccess = true;
                Data = data ;
            }
        }
    }
}