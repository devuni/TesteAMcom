using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using Domain.Entities;
using System;

namespace Application.Commands
{
    public class MovimentacaoHandler : IRequestHandler<MovimentacaoCommand, IActionResult>
    {
        private readonly IContaCorrenteRepository _repository;
        
        public MovimentacaoHandler(IContaCorrenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
        {
            var conta = await _repository.ObterPorIdAsync(request.IdContaCorrente);

            if (conta == null) return new BadRequestObjectResult(new { erro = "INVALID_ACCOUNT" });
            if (!conta.Ativo) return new BadRequestObjectResult(new { erro = "INACTIVE_ACCOUNT" });
            if (request.Valor <= 0) return new BadRequestObjectResult(new { erro = "INVALID_VALUE" });
            if (request.Tipo != "C" && request.Tipo != "D") return new BadRequestObjectResult(new { erro = "INVALID_TYPE" });
            
            var movimento = new Movimento
            {
                Id = Guid.NewGuid().ToString(),
                IdContaCorrente = request.IdContaCorrente,
                DataMovimento = DateTime.UtcNow.ToString("dd/MM/yyyy"),
                TipoMovimento = request.Tipo,
                Valor = request.Valor
            };
            
            await _repository.RegistrarMovimentoAsync(movimento);
            return new OkObjectResult(new { idmovimento = movimento.Id });
        }
    }
}
