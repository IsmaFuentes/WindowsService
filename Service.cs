using System;
using System.Threading;
using System.Configuration;
using System.ServiceProcess;

namespace WindowsService
{
  public partial class Service : ServiceBase
  {
    private const int TIMER_MILISECONDS = 10000;

    public Service()
    {
      InitializeComponent();
    }

    private Timer backgroundProcess { get; set; }

    public void OnDebug()
    {
      OnStart(null);
    }

    protected override void OnStart(string[] args)
    {
      backgroundProcess = new Timer(new TimerCallback(OnTimer), null, 1000, Timeout.Infinite);
    }

    protected override void OnStop()
    {
      if(backgroundProcess != null)
      {
        backgroundProcess.Change(Timeout.Infinite, Timeout.Infinite);
        backgroundProcess.Dispose();
        backgroundProcess = null;
      }
    }

    private void OnTimer(object state)
    {
      try
      {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($" OnTimer - {DateTime.Now}");
#endif

        // Do work...
      } 
      catch(Exception ex)
      {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"[ERROR]: {ex.Message}");
#endif
      }

      // Restart timer
      backgroundProcess.Change(GetMiliseconds(), Timeout.Infinite);
    }

    private int GetMiliseconds()
    {
      try
      {
        int seconds = int.Parse(ConfigurationManager.AppSettings["Periodicity"].ToString());

        return seconds * 1000;
      }
      catch
      {
        return TIMER_MILISECONDS;
      }
    }
  }
}
