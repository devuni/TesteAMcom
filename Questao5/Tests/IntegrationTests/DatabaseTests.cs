using System.Data;
using Dapper;
using FluentAssertions;
using Infrastructure.Database;
using Microsoft.Data.Sqlite;

public class DatabaseTests : IDisposable
{
    private readonly IDbConnection _dbConnection;

   public DatabaseTests()
{
    // Criar conexão SQLite em memória
    _dbConnection = new SqliteConnection("Data Source=:memory:");
    _dbConnection.Open();

    // Criar tabelas no banco de dados de testes
    DatabaseBootstrap.Initialize(_dbConnection);

    // Garantir que os dados iniciais sejam inseridos
    InserirDadosIniciais();
}

private void InserirDadosIniciais()
{
    var insertQuery = @"
        INSERT INTO contacorrente (idcontacorrente, numero, nome, ativo) VALUES
        ('B6BAFC09-6967-ED11-A567-055DFA4A16C9', 123, 'Katherine Sanchez', 1),
        ('FA99D033-7067-ED11-96C6-7C5DFA4A16C9', 456, 'Eva Woodward', 1),
        ('382D323D-7067-ED11-8866-7D5DFA4A16C9', 789, 'Tevin Mcconnell', 1),
        ('F475F943-7067-ED11-A06B-7E5DFA4A16C9', 741, 'Ameena Lynn', 0),
        ('BCDACA4A-7067-ED11-AF81-825DFA4A16C9', 852, 'Jarrad Mckee', 0),
        ('D2E02051-7067-ED11-94C0-835DFA4A16C9', 963, 'Elisha Simons', 0);
    ";

    _dbConnection.Execute(insertQuery);
}

    public void Dispose()
    {
        _dbConnection.Close();
    }

    [Fact]
    public async Task Deve_Inserir_Contas_Correntes_Iniciais()
    {
        // Act
        var contas = await _dbConnection.QueryAsync("SELECT * FROM contacorrente");

        // Assert
        contas.Should().NotBeEmpty();
        contas.Should().HaveCount(6); // Deve ter os 6 registros inseridos no banco
    }

    [Fact]
public async Task Deve_Registrar_Uma_Nova_Movimentacao()
{
    // Arrange
    var idContaValida = "B6BAFC09-6967-ED11-A567-055DFA4A16C9"; // Conta já cadastrada

    var movimento = new
    {
        idmovimento = Guid.NewGuid().ToString(),
        idcontacorrente = idContaValida,
        datamovimento = DateTime.Now.ToString("dd/MM/yyyy"),
        tipomovimento = "C", // Crédito
        valor = 500.00m
    };

    var insertQuery = @"
        INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
        VALUES (@idmovimento, @idcontacorrente, @datamovimento, @tipomovimento, @valor)";

    // Act
    await _dbConnection.ExecuteAsync(insertQuery, movimento);

    // Assert
    var result = await _dbConnection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM movimento");
    result.Should().Be(1);
}

    [Fact]
public async Task Deve_Consultar_Saldo_De_Conta_Corrente()
{
    // Arrange
    var idConta = "B6BAFC09-6967-ED11-A567-055DFA4A16C9"; // Conta já cadastrada

    await _dbConnection.ExecuteAsync(@"
        INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
        VALUES (@id, @conta, @data, @tipo, @valor)", new
    {
        id = Guid.NewGuid().ToString(),
        conta = idConta,
        data = DateTime.Now.ToString("dd/MM/yyyy"),
        tipo = "C",
        valor = 1000.00m
    });

    // Act
    var saldo = await _dbConnection.QueryFirstOrDefaultAsync<decimal>(
        "SELECT COALESCE(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END), 0) FROM movimento WHERE idcontacorrente = @conta",
        new { conta = idConta });

    // Assert
    saldo.Should().Be(1000.00m);
}
}
