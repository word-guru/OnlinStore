using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Json;
using OnlinStore;
using OnlinStore.Interface;
using OnlinStore.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.WriteIndented = true; }); //настройка вывода JSON

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<ICatalog, InMemoryCatalog>();
    builder.Services.AddSingleton<IClock,TimeInUTC>();
    builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SmtpConfig"));
//builder.Services.Configure<SmtpConfig>(AddUserSecrets.ConfigureAppConfiguration);
    builder.Services.AddScoped<IEmailSender, MailKitEmailSender>();
    builder.Services.AddHostedService<SenderEmailBackgroundService>();
    builder.Host.UseSerilog((ctx, conf) =>
    {
        conf
            .WriteTo.Console()
            .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
            .ReadFrom.Configuration(ctx.Configuration)
            ;
    });


    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSerilogRequestLogging();

//var catalog = app.Services.GetService<ICatalog>();

    async Task<string> SendMail(IEmailSender sender, string message, string subject)
    {
        await sender.SendAsync("PV011", "legeon48@mail.ru", subject, message);

        return "Ok";
    }

    app.MapGet("/sendmail", SendMail);

    app.MapGet("/catalog", (ICatalog catalog) =>
    {
        return catalog.GetProducts();
    });

    app.MapPost("/catalog/add_product", (ICatalog catalog,Product product, HttpContext context) =>
    {
        catalog.AddProduct(product);
        context.Response.StatusCode = 201;
    });

    app.MapPost("/catalog/clear_product", (ICatalog catalog,HttpContext context) =>
    {
        catalog.ClearProduct();
        context.Response.StatusCode = 202;
    });
    app.MapGet("/time", (IClock time) => time.GetUTCTime());

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush(); //перед выходом дожидаемся пока все логи будут записаны
}

