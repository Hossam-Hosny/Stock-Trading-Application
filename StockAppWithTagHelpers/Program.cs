using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts.Helpers;
using ServiceContracts.Interface;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));

builder.Services.AddTransient(typeof(IFinnhubService), typeof(FinnhubService));
builder.Services.AddTransient(typeof(IStockService), typeof(StocksService));
builder.Services.AddTransient(typeof(IFinnhubRepository), typeof(FinnhubRepository));
builder.Services.AddTransient(typeof(IStocksRepositories), typeof(StocksRepository));

builder.Services.AddHttpClient();


builder.Services.AddDbContext<StockMarketDbContext>( options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefalutConnectionString"));
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
