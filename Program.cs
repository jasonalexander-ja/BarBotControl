using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Channels;
using Blazored.LocalStorage;
using BarBotControl.Data;
using BarBotControl.Config;
using BarBotControl.Services;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var serverVersion = new MySqlServerVersion(new Version(11, 2, 2));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

builder.Services.AddSingleton(
    builder.Configuration.GetRequiredSection("SudoUserConfig")
    .Get<SudoUserConfig>());
builder.Services.AddSingleton<SessionService>();

builder.Services.AddScoped<SudoUserDataService>();
builder.Services.AddScoped<SudoUserService>();

builder.Services.AddScoped<ModuleDataService>();
builder.Services.AddScoped<ModuleService>();

builder.Services.AddScoped<OptionDataService>();
builder.Services.AddScoped<OptionService>();

builder.Services.AddScoped<ErrorTypeDataService>();
builder.Services.AddScoped<ErrorTypeService>();




var concurrentService = new ConcurrentService();

builder.Services.AddSingleton(concurrentService);





// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
