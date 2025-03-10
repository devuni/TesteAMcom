using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Queries;
using System.Threading.Tasks;

namespace Api.Controllers{

    [ApiController]
    [Route("api/[controller]")]
    public class SaldoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SaldoController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{idContaCorrente}")]
        public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
        {
            return await _mediator.Send(new SaldoQuery(idContaCorrente));
        }
    }
}