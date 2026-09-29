using MaterialBalanceAPI.Services;
using Serilog;
using Microsoft.EntityFrameworkCore;
using MaterialBalanceAPI.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PlantDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IDataOrchestratorService, DataOrchestratorService>();

// 1. Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Уровень логирования
    .WriteTo.Console()    // Дублируем в консоль
    .WriteTo.File("logs/simulation-log-.txt",
        rollingInterval: RollingInterval.Day, // Новый файл каждый день
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

// Добавляем сервисы проверок состояния для K8s
builder.Services.AddHealthChecks();

// Добавляем логирование в консоль и отладчик
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// 2. Подключаем Serilog к хосту
builder.Host.UseSerilog();

// Add services to the container.
// Регистрируем наш сервис
builder.Services.AddScoped<IBalanceSolverService, BalanceSolverService>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.WithOrigins("http://localhost:5173") // Адрес фронтенда
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.UseDefaultFiles(); // Позволяет открывать index.html по умолчанию
app.UseStaticFiles();  // Разрешает отдачу файлов из wwwroot

app.MapHealthChecks("/api/health/live"); // Эндпоинт для проверки состояния
app.MapHealthChecks("/api/health/ready"); // Эндпоинт для проверки готовности

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PlantDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при миграции базы данных.");
    }
}

try
{
    Log.Information("Запуск веб-сервера расчета материального баланса...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Приложение завершилось с критической ошибкой");
}
finally
{
    Log.CloseAndFlush(); // Гарантируем запись всех логов перед выходом
}