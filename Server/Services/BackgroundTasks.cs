public class BackgroundTasks : BackgroundService
{
    private readonly PeriodicTimer _timer =new(TimeSpan.FromMinutes(1));
    ILogger<BackgroundTasks> _logger;
    public IServiceProvider Services { get; }

    public BackgroundTasks(ILogger<BackgroundTasks> logger, IServiceProvider services)
    {
        _logger = logger;
        Services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Background Hosted Service running.");
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)        
            await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Service Hosted Service is working.");

        using (var scope = Services.CreateScope())
        {
            var dedicatedSMS = 
                scope.ServiceProvider
                    .GetRequiredService<IDedicatedSMS>();

            await dedicatedSMS.DoWork();
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Service Hosted Service is stopping.");
        _timer!.Dispose();
        await base.StopAsync(stoppingToken);
    }    
}