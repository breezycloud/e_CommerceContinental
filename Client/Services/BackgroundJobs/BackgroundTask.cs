using System.Timers;
using e_CommerceContinental.Client.Services;

public class JobExecutedEventArgs : EventArgs {}
public class DownloadJobExecutedEventArgs : EventArgs {}

public class BackgroundTask : HostedService, IDisposable
{
    bool _Running, _DownloadTaskRunning;
    System.Timers.Timer? _Timer;    
    System.Timers.Timer? _DownloadTaskTimer;    
    PeriodicTimer? _PeriodicTimer;
    public event EventHandler<JobExecutedEventArgs>? JobExecuted;
    public event EventHandler<DownloadJobExecutedEventArgs>? DownloadTaskExecuted;
    void OnJobExecuted()
    {
        JobExecuted!.Invoke(this, new JobExecutedEventArgs());
    }
    void OnDonwloadTaskJobExecuted()
    {
        DownloadTaskExecuted!.Invoke(this, new DownloadJobExecutedEventArgs());
    }    
    public void StartExecuting()
    {
        if (!_Running)
        {
            _Timer = new System.Timers.Timer();
            _Timer.Interval = 1000;
            _Timer.Elapsed += HandleTimer!;
            _Timer.AutoReset = true;
            _Timer.Enabled = true;

            _Running = true;
        }

        if (!_DownloadTaskRunning)
        {
            _DownloadTaskTimer = new System.Timers.Timer();
            _DownloadTaskTimer.Interval = 600000;
            _DownloadTaskTimer.Elapsed += HandleDownloadTaskTimer!;
            _DownloadTaskTimer.AutoReset = true;
            _DownloadTaskTimer.Enabled = true;

            _DownloadTaskRunning = true;
        }        
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (await _PeriodicTimer!.WaitForNextTickAsync(token))
        {
            Console.WriteLine("I'm running in background!!!");
            await Task.Delay(100);
        }
    }

    private void HandleDownloadTaskTimer(object sender, ElapsedEventArgs e)
    {
        OnDonwloadTaskJobExecuted();
    }

    void HandleTimer(object source, ElapsedEventArgs e)
    {
        OnJobExecuted();
    }

    public void Dispose()
    {
        if (_Running)
            _Timer = null;

        if (_DownloadTaskRunning)
            _DownloadTaskTimer = null;
    }
}