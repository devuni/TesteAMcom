using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> ObterPorIdAsync(string id);
        Task<decimal> ObterSaldoAsync(string id);
        Task RegistrarMovimentoAsync(Movimento movimento);
    }
}