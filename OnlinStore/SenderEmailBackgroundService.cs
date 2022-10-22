using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Bcpg;
using Polly;
using Polly.Retry;

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
        
        Console.WriteLine("Server started successfully at " + scopeCurrentTime.GetUTCTime());

        while (!stoppingToken.IsCancellationRequested)
        {
            var to = "legeon48@mail.ru";
            const int retryCount = 2;

            AsyncRetryPolicy policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync( retryCount, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryAttempt, context) => {
                        _logger.LogWarning(exception, "Error while sending email. Retrying: {Attempt}", retryAttempt);
                    }
                );

            PolicyResult? result = await policy.ExecuteAndCaptureAsync(
                token => scopeSendMessage.SendAsync("PV011", to, "Server testing",
                    "Server is working properly" +
                    " Total memory: " +
                    GC.GetTotalMemory(false) + " bytes"
                ), stoppingToken);

            if (result.Outcome == OutcomeType.Failure)
            {
                _logger.LogError(result.FinalException, "There was an error while sending email");
            }

            
            await Task.Delay(new TimeSpan(1, 0, 0));
        }
    }

    /*protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       await using var scope = _serviceProvider.CreateAsyncScope();
        var scopeCurrentTime = scope.ServiceProvider.GetRequiredService<IClock>();
        var scopeSendMessage = scope.ServiceProvider.GetRequiredService<IEmailSender>();
        
        Console.WriteLine("Server started successfully at " + scopeCurrentTime.GetUTCTime());

        while (!stoppingToken.IsCancellationRequested)
        {
            var to = "legeon48@mail.ru";
            bool sendingSucceeded = false;
            const int attemptsLimit = 2;
            
            for(int attemptsCount = 1;!sendingSucceeded && attemptsCount <= attemptsLimit;attemptsCount++)
            {
                try
                {
                    await scopeSendMessage.SendAsync("PV011", to, "Server testing",
                        "Server is working properly" +
                        " Total memory: " +
                        GC.GetTotalMemory(false) + " bytes"
                    );
                    sendingSucceeded = true;
                }
                catch (Exception e) when (attemptsCount < attemptsLimit)
                {

                    _logger.LogWarning(e,
                        "При попытке отправить письмо произошла ошибка, сервис:  {Service}, {Recipient}." +
                        "Попытка #{Attempt}", scopeSendMessage.GetType(), to, attemptsCount);
                }
                catch (Exception e)
                {
                    _logger.LogError(e,
                        "Отправка письма завершилась с ошибкой, сервис: {Service}, {Recipient}." +
                        "Попытка #{Attempt}", scopeSendMessage.GetType(), to, attemptsCount);
                }
            } 
            
            await Task.Delay(new TimeSpan(1, 0, 0));
        }
    }*/
    
}
