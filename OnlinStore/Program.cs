using System.Reflection;
using Microsoft.AspNetCore.Http.Json;
using OnlinStore;
using OnlinStore.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.WriteIndented = true; }); //настройка вывода JSON

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var catalog = new Catalog();

//app.MapGet("/", () => "Hello World!");
app.MapGet("/catalog", () =>
{
    return catalog.GetProducts();
});

app.MapPost("/catalog/add_product", (Product product, HttpContext context) =>
{
    catalog.AddProduct(product);
    context.Response.StatusCode = 201;
});

app.MapPost("/catalog/clear_product", (HttpContext context) =>
{
    catalog.ClearProduct();
    context.Response.StatusCode = 202;
});


app.Run();