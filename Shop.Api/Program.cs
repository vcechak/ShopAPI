using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Shop.Kafka.Messaging.Interfaces;
using Shop.Kafka.Messaging;
using Shop.DataAccess.Data;
using Shop.DataAccess.Models;
using Shop.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// Configure Services
// -------------------------------

// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlite("Data Source=../Shop.db")); // Change path if needed

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kafka
builder.Services.AddSingleton(new ProducerConfig
{
    BootstrapServers = "localhost:9092"
});
builder.Services.AddScoped(typeof(IKafkaProducer<>), typeof(KafkaProducer<>));

// Other Services
builder.Services.AddScoped<IOrderNumberGenerator, OrderNumberGenerator>();

var app = builder.Build();

// -------------------------------
// Configure Middleware
// -------------------------------

if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// -------------------------------
// Ensure DB Initialization
// -------------------------------

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();

    if (db.Database.EnsureCreated())
    {
        db.Add(new OrderNumberSequence
        {
            Date = DateTime.UtcNow.Date,
            Counter = 0
        });
        db.SaveChanges();
    }
}

// -------------------------------
// Run the Application
// -------------------------------

app.Run();
