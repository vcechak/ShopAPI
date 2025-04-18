using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Shop.Kafka.Messaging.Interfaces;
using Shop.Kafka.Messaging;
using Shop.DataAccess.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlite("Data Source=../Shop.db")); // You can change the file name
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kafka
builder.Services.AddSingleton<ProducerConfig>(new ProducerConfig
{
    BootstrapServers = "localhost:9092"
});

builder.Services.AddScoped(typeof(IKafkaProducer<>), typeof(KafkaProducer<>));

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
