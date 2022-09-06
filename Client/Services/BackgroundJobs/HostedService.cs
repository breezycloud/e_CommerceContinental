namespace e_CommerceContinental.Client.Services;

public abstract class HostedService
{
    protected abstract Task ExecuteAsync(CancellationToken stoppingToken);
}