using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Shop.Contracts;
using Shop.DataAccess.Data;
using Shop.DataAccess.Data.Repository;
using Shop.Kafka.Consumer;
using Shop.Kafka.Messaging;
using Shop.Kafka.Messaging.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

// Register PaymentCheckWorker as Scoped
builder.Services.AddScoped<PaymentCheckWorker>();

var orderApiBaseUrl = Environment.GetEnvironmentVariable("OrderApiBaseUrl")
                      ?? builder.Configuration["ApiSettings:OrderApiBaseUrl"];

if (string.IsNullOrEmpty(orderApiBaseUrl))
{
    throw new InvalidOperationException("OrderApiBaseUrl is not configured.");
}

builder.Services.AddHttpClient("OrderApi", client =>
{
    client.BaseAddress = new Uri(orderApiBaseUrl);
});

builder.Services.AddSingleton<ConsumerConfig>(new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "payment-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
});

builder.Services.AddSingleton<IKafkaConsumer<PaymentRequestMessage>, KafkaConsumer<PaymentRequestMessage>>();

// Register IOrderRepository as Scoped
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlite("Data Source=../Shop.db")); // You can change the file name
builder.Services.AddScoped<ShopDbContext, ShopDbContext>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var host = builder.Build();

// Manually start the scoped worker
using var scope = host.Services.CreateScope();
var worker = scope.ServiceProvider.GetRequiredService<PaymentCheckWorker>();
await worker.StartAsync(CancellationToken.None);

host.Run();