using System.Reflection;
using Microsoft.AspNetCore.Http.Json;
using OnlinStore;
using OnlinStore.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.WriteIndented = true; }); //настройка вывода JSON

var app = builder.Build();

var catalog = new Catalog();

app.MapGet("/", () => "Hello World!");
app.MapGet("/catalog", () =>
{
    return catalog.Products;
});

app.MapPost("/catalog/add_product", (Product product, HttpContext context) =>
{
    catalog.Products.Add(product);
    context.Response.StatusCode = 201;
});

app.MapPost("/catalog/clear", (HttpContext context) =>
{
    catalog.Products.Clear();
    context.Response.StatusCode = 202;
});


app.Run();