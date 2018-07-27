namespace MyWebAPI.Filters.Security.Interface
{
    /// <summary>
    /// 注册信息操作
    /// </summary>
    public interface IRegister
    {
        /// <summary>
        /// 获取注册信息
        /// </summary>
        /// <param name="appId">注册Id</param>
        /// <returns>注册信息实体</returns>
        RegisterInfo GetRegister(string appId);

    }
}
