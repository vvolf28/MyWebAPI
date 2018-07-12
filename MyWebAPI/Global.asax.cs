using System.IO;
using System.Web;
using System.Web.Http;

namespace MyWebAPI
{
    /// <summary>
    /// api应用
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        /// 服务启动
        /// </summary>
        protected void Application_Start()
        {
            LoadLog4netConfig();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        /// <summary>
        /// 加载Log4net配置项
        /// </summary>
        private void LoadLog4netConfig()
        {
            string filePath = Server.MapPath("~/App_Data/log4net.config");
            var fileInfo = new FileInfo(filePath);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(fileInfo);
        }
    }
}
