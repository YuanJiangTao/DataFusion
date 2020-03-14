using System.Linq;
using System.ServiceProcess;

namespace DataFusion.Interfaces.Utils
{
    public class ServiceUtil
    {
        public static bool CheckServiceIsExist(string serviceName)
        {
            var serviceController = ServiceController.GetServices().ToList();
            return serviceController.Exists(p => p.ServiceName == serviceName);
        }
        public static bool CheckServiceIsRunning(string serviceName)
        {
            if (CheckServiceIsExist(serviceName))
            {
                return ServiceController.GetServices().FirstOrDefault(p => p.ServiceName == serviceName)?.
                    Status == ServiceControllerStatus.Running;
            }
            return false;
        }
    }
}
