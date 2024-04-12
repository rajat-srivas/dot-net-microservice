using Ordering.API;
using Ordering.Application;

var builder = WebApplication.CreateBuilder(args);

// add services to the container.

builder.Services
	.AddApplicationServices()
	.AddInfrastructureServices(builder.Configuration)
	.AddAiServices();

var app = builder.Build();

// configure the HHTP request pipeline

app.Run();
