var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddSecretsManager(configurator: opt =>
{
    opt.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}");
    opt.KeyGenerator = (_, s) => s
        .Replace($"{env}_{appName}", string.Empty)
        .Replace("__", ":");
    opt.PollingInterval = TimeSpan.FromSeconds(10);
});

builder.Services.AddSwaggerGen();

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