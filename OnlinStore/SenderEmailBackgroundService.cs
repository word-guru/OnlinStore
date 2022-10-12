namespace OnlinStore;

public class SenderEmailBackgroundService : BackgroundService
{
    
    private readonly IServiceProvider _serviceProvider;

    public SenderEmailBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var scopeCurrentTime = scope.ServiceProvider.GetRequiredService<IClock>();
        var scopeSendMessage = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            
        Console.WriteLine("Server started successfully at " + scopeCurrentTime.GetUTCTime());

        while (!stoppingToken.IsCancellationRequested)
        {
            scopeSendMessage.Send("PV011",
                "legeon48@mail.ru",
                "Server testing",
                "Server is working properly" +
                "<br>Total memory: " + 
                GC.GetTotalMemory(false) + " bytes"
            );
            
            await Task.Delay(new TimeSpan(1, 0, 0));
        }
    }
    
}