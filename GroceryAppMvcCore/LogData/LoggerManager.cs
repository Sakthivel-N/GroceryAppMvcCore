using log4net;
using log4net.Config;
using System.Reflection;
using System.Xml;



namespace GroceryAppMvcCore.LogData
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(LoggerManager));
        XmlDocument xmlDocument = new XmlDocument();
        public void LoginInfo(string message)
        {
           
            xmlDocument.Load(File.OpenRead("GroceryLog.config"));
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                        typeof(log4net.Repository.Hierarchy.Hierarchy));

            XmlConfigurator.Configure(repo, xmlDocument["log4net"]);

            _logger.Info(message);
        }
    }

}





