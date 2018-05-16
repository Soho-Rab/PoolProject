using System.IO;
using log4net;
using log4net.Repository;
using log4net.Config;



namespace Pool.Common
{
    public class Logs
    {
        /*public static void SetConsoleLogger() => InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));*/
        private static ILog log = null;

        public static ILog Logger
        {
            get
            {
                if (log == null)
                {
                    ILoggerRepository repository = LogManager.CreateRepository("NetCoreRepository");
                    XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));
                    log= LogManager.GetLogger(repository.Name, "NetCorelog4net");
                    return log;
                }
                else
                {
                    return log;
                }
            }
        }
    }
}
