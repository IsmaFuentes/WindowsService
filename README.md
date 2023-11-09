## Windows Service with Timer - Template Project

### Debugging

The project will work as a Console app when compiled with the DEBUG flag, making it easier to debug

- Other debugging methods involve attaching the project to a currently running service process, which has its own set of limitations and annoyances

```csharp
  internal static class Program
  {
    static void Main()
    {
#if DEBUG
      // Create the Service instance and call "OnDebug" method which internally calls "OnStart(null)"
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
```

### Background timer

A background timer will be registered when the Service is executed for the first time

- The timer will fire it's callback after 1 second and wont be repeating the process until we notify it to run again
```csharp
    protected override void OnStart(string[] args)
    {
      backgroundProcess = new Timer(new TimerCallback(OnTimer), null, 1000, Timeout.Infinite);
    }
```

- The operation will take as much time as it needs to finish, then we can notify the timer to start again after "X" seconds


```csharp
    private void OnTimer(object state)
    {
      try
      {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($" OnTimer - {DateTime.Now}");
#endif

        // Long running operation...
      } 
      catch(Exception ex)
      {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"[ERROR]: {ex.Message}");
#endif
      }

      // Restart the timer
      backgroundProcess.Change(GetMiliseconds(), Timeout.Infinite);
    }
```

### Installation

There Service comes with a ProjectInstaller, which will be executed when installing the service using NET Framework's **installutil** utility.

- Manual installation/uninstallation

```bash
# install
cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319\
/k installutil "path to service executable"

# uninstall
cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319\
/k installutil /u "path to service executable"
```

- The project comes with two .bat files that perform the installation and uninstallation processes. The **%serviceName%** parameter should be renamed to match the real executable's name.