using Amazon;
using Amazon.Internal;
using SecretManagement.API;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddSecretsManager(region: RegionEndpoint.EUCentral1, configurator:
    opt =>
    {
        opt.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}_");
        opt.KeyGenerator = (entry, s) => s.Replace($"{env}_{appName}_", string.Empty)
            .Replace("__", ":");
        opt.PollingInterval = TimeSpan.MinValue;
    });

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(DatabaseSettings.SectionName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();