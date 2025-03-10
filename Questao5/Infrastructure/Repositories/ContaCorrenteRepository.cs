using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        
        public ContaCorrenteRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        
        public async Task<ContaCorrente> ObterPorIdAsync(string id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>("SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = id });
        }
        
        public async Task<decimal> ObterSaldoAsync(string id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var creditos = await connection.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'C'", new { Id = id });
            var debitos = await connection.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'D'", new { Id = id });
            return creditos - debitos;
        }
        
        public async Task RegistrarMovimentoAsync(Movimento movimento)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            await connection.ExecuteAsync("INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@Id, @Conta, @Data, @Tipo, @Valor)",
                new { Id = movimento.Id, Conta = movimento.IdContaCorrente, Data = movimento.DataMovimento, Tipo = movimento.TipoMovimento, Valor = movimento.Valor });
        }
    }
}
