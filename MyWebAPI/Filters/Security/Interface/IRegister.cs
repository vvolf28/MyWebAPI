using MyWebAPI.Filters.Security.Entity;
using System.Collections.Generic;

namespace MyWebAPI.Filters.Security.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRegister
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        RegisterInfo GetRegister(string appId);

    }
}
