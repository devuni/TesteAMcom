using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands
{
    public record MovimentacaoCommand(string IdRequisicao, string IdContaCorrente, decimal Valor, string Tipo) : IRequest<IActionResult>;
}