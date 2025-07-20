using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Interfaces;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Kafka;
using TransactionService.Infrastructure.Kafka.Producers;
using TransactionService.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with PostgreSQL
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITransactionService, TransactionService.Application.Services.TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Register KafkaProducer
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>(sp =>
{
    var config = new Confluent.Kafka.ProducerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"] ?? "kafka:9092"
    };
    return new KafkaProducer(config);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
    db.Database.Migrate();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
