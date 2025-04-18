using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Shop.Contracts;
using Shop.DataAccess.Data;
using Shop.DataAccess.Data.Repository;
using Shop.Kafka.Consumer;
using Shop.Kafka.Messaging;
using Shop.Kafka.Messaging.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

// -------------------------------
// Configuration
// -------------------------------

// Get Order API Base URL
var orderApiBaseUrl = Environment.GetEnvironmentVariable("OrderApiBaseUrl")
                      ?? builder.Configuration["ApiSettings:OrderApiBaseUrl"];

if (string.IsNullOrEmpty(orderApiBaseUrl))
{
    throw new InvalidOperationException("OrderApiBaseUrl is not configured.");
}

// -------------------------------
// Service Registration
// -------------------------------

// Register PaymentCheckWorker as Scoped
builder.Services.AddScoped<PaymentCheckWorker>();

// Register HttpClient for Order API
builder.Services.AddHttpClient("OrderApi", client =>
{
    client.BaseAddress = new Uri(orderApiBaseUrl);
});

// Kafka Consumer
builder.Services.AddSingleton(new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "payment-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
});
builder.Services.AddSingleton<IKafkaConsumer<PaymentCheckRequestMessage>, KafkaConsumer<PaymentCheckRequestMessage>>();

// Database Configuration
builder.Services
    .AddDbContext<ShopDbContext>(options =>
        options.UseSqlite("Data Source=../Shop.db"))
    .AddScoped<ShopDbContext>()
    .AddScoped<IOrderRepository, OrderRepository>();

// -------------------------------
// Build Host
// -------------------------------
var host = builder.Build();

// -------------------------------
// Start Worker
// -------------------------------

using (var scope = host.Services.CreateScope())
{
    var worker = scope.ServiceProvider.GetRequiredService<PaymentCheckWorker>();
    await worker.StartAsync(CancellationToken.None);
}

// -------------------------------
// Run Application
// -------------------------------

host.Run();