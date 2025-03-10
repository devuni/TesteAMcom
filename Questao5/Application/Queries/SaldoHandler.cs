using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using System;

namespace Application.Queries
{
    public class SaldoHandler : IRequestHandler<SaldoQuery, IActionResult>
    {
        private readonly IContaCorrenteRepository _repository;
        
        public SaldoHandler(IContaCorrenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Handle(SaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _repository.ObterPorIdAsync(request.IdContaCorrente);
            if (conta == null) return new BadRequestObjectResult(new { erro = "INVALID_ACCOUNT" });
            if (!conta.Ativo) return new BadRequestObjectResult(new { erro = "INACTIVE_ACCOUNT" });
            
            var saldo = await _repository.ObterSaldoAsync(request.IdContaCorrente);
            
            return new OkObjectResult(new
            {
                conta.Numero,
                conta.Nome,
                DataHora = DateTime.UtcNow,
                Saldo = saldo
            });
        }
    }
}