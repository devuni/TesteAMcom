using Microsoft.OpenApi.Models;
using Infrastructure.Database;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuração do appsettings.json
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");

// Configuração dos serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Banco API", Version = "v1" });
});

// Configuração do SQLite e Dapper
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
builder.Services.AddSingleton<IContaCorrenteRepository, ContaCorrenteRepository>();

// Configuração do MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banco API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Inicialização do banco de dados
using (var scope = app.Services.CreateScope())
{
    var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
    using var dbConnection = dbConnectionFactory.CreateConnection();
    DatabaseBootstrap.Initialize(dbConnection);
}

app.Run();

public partial class Program { }
