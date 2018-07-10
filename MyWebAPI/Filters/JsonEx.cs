using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebAPI.Filters
{
    public class JsonEx
    {
        private static readonly JsonSerializerSettings m_JsonSetting = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        /// <summary>
        /// 对象 to Json (CamelCase格式)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, m_JsonSetting);
        }


        /// <summary>
        /// Json To 对象 (CamelCase格式)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJson<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr, m_JsonSetting);
        }
    }
}