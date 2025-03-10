using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Queries
{
    public record SaldoQuery(string IdContaCorrente) : IRequest<IActionResult>;
}