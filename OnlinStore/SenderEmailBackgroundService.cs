namespace OnlinStore;

public class SenderEmailBackgroundService : BackgroundService
{
    
    private readonly IServiceProvider _serviceProvider;
   
    private readonly ILogger<SenderEmailBackgroundService> _logger;

    public SenderEmailBackgroundService(IServiceProvider serviceProvider,ILogger<SenderEmailBackgroundService> logger,
                                        IHostApplicationLifetime applicationLifetime)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        applicationLifetime.ApplicationStarted.Register(() => {_logger.LogInformation("The server has been successfully started");});
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       await using var scope = _serviceProvider.CreateAsyncScope();
        var scopeCurrentTime = scope.ServiceProvider.GetRequiredService<IClock>();
        var scopeSendMessage = scope.ServiceProvider.GetRequiredService<IEmailSender>();
        var to = "legeon48@mail.ru";

        Console.WriteLine("Server started successfully at " + scopeCurrentTime.GetUTCTime());

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await scopeSendMessage.SendAsync("PV011",to,"Server testing",
                    "Server is working properly" +
                    " Total memory: " + 
                    GC.GetTotalMemory(false) + " bytes"
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e,"there was an error when sending the message, service: {Service}, {Recipient}",scopeSendMessage.GetType());
            }
            
            await Task.Delay(new TimeSpan(1, 0, 0));
        }
    }
    
}
