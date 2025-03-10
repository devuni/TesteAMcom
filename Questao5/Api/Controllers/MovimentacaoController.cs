using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Commands;
using System.Threading.Tasks;


namespace Api.Controllers{

    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MovimentacaoController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentacaoCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}