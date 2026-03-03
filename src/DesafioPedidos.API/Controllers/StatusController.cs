using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPedidos.API.Controllers;

[ApiController]
[Route("api/status")]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessarStatus([FromBody] StatusRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _statusService.ProcessarAsync(request, cancellationToken);
        return Ok(result);
    }
}
