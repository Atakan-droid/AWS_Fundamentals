using Amazon.SQS;
using Customers.Consumer;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
//builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddScoped<GuidService>();

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();