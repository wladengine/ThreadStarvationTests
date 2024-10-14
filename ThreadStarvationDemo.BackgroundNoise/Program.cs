using ThreadStarvationDemo.BackgroundNoise;
using ThreadStarvationDemo.BackgroundNoise.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddHostedService<CpuBoundedConsumer>();
//builder.Services.AddHostedService<TplConsumer>();

builder.Services.Configure<ProductDatabaseSettings>(builder.Configuration.GetSection(nameof(ProductDatabaseSettings)));

builder.Services.AddScoped<ProductsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("PID: {pid}", Environment.ProcessId);

app.Run();
