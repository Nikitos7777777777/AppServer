using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using ServerApp.CRUDdb;
using ServerApp.Models;
using ServerApp.Services;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MessengerDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<CRUDUser>();
builder.Services.AddScoped<CRUDMessanges>();
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<ChatService>();
app.MapGrpcService<ÑonnectionServer>();
app.MapGrpcService<ClientDataServer>();

app.Run();

