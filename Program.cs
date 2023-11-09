using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main()
    {
#if DEBUG
      Service service = new Service();
      service.OnDebug();
      System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
      ServiceBase[] ServicesToRun;
      ServicesToRun = new ServiceBase[]
      {
        new Service()
      };
      ServiceBase.Run(ServicesToRun);
#endif
    }
  }
}
