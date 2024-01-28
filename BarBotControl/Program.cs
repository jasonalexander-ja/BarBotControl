using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using Blazored.LocalStorage;
using BarBotControl.Extensions;


var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

builder.AddDatabase();
builder.AddDataServices();

var isMsalEnmabled = builder.Configuration.GetSection("EnableMSALForTesting")
        .Get<bool>();

if (isMsalEnmabled)
{
	builder.AddMicrosoftAuth();
}

builder.AddSudoUserService();

builder.AddWorker();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();


var app = builder.Build();

// The protocol doesn't get set from the forwarding header middleware when running behind 
// a reverse proxy in a Docker Container for some reason 
var shouldForceHttps = Environment.GetEnvironmentVariable("FORCE_HTTPS") ?? "";
if (shouldForceHttps == "true" && isMsalEnmabled)
{
	app.Use((context, next) =>
	{
		context.Request.Scheme = "https";
		return next(context);
	});
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

if (isMsalEnmabled)
{
    app.UseForwardedHeaders();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

if (isMsalEnmabled)
{
	app.UseAuthentication();
	app.UseAuthorization();
}

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
