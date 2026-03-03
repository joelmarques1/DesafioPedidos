using DesafioPedidos.Application.Commands.AtualizarPedido;
using DesafioPedidos.Application.Commands.CriarPedido;
using DesafioPedidos.Application.Commands.DeletarPedido;
using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Application.Queries.BuscarPedido;
using DesafioPedidos.Application.Queries.ListarPedidos;
using DesafioPedidos.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPedidos.API.Controllers;

[ApiController]
[Route("api/pedido")]
public class PedidoController : ControllerBase
{
    private readonly ICriarPedidoHandler _criarHandler;
    private readonly IAtualizarPedidoHandler _atualizarHandler;
    private readonly IDeletarPedidoHandler _deletarHandler;
    private readonly IBuscarPedidoHandler _buscarHandler;
    private readonly IListarPedidosHandler _listarHandler;

    public PedidoController(
        ICriarPedidoHandler criarHandler,
        IAtualizarPedidoHandler atualizarHandler,
        IDeletarPedidoHandler deletarHandler,
        IBuscarPedidoHandler buscarHandler,
        IListarPedidosHandler listarHandler)
    {
        _criarHandler = criarHandler;
        _atualizarHandler = atualizarHandler;
        _deletarHandler = deletarHandler;
        _buscarHandler = buscarHandler;
        _listarHandler = listarHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var pedidos = await _listarHandler.HandleAsync(new ListarPedidosQuery(), cancellationToken);
        return Ok(pedidos);
    }

    [HttpGet("{numeroPedido}")]
    public async Task<IActionResult> Buscar(string numeroPedido, CancellationToken cancellationToken)
    {
        try
        {
            var pedido = await _buscarHandler.HandleAsync(new BuscarPedidoQuery(numeroPedido), cancellationToken);
            return Ok(pedido);
        }
        catch (PedidoNaoEncontradoException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] PedidoRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CriarPedidoCommand(request.Pedido, request.Itens);
            var result = await _criarHandler.HandleAsync(command, cancellationToken);
            return CreatedAtAction(nameof(Buscar), new { numeroPedido = result.Pedido }, result);
        }
        catch (PedidoDuplicadoException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{numeroPedido}")]
    public async Task<IActionResult> Atualizar(string numeroPedido, [FromBody] PedidoRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new AtualizarPedidoCommand(numeroPedido, request.Itens);
            var result = await _atualizarHandler.HandleAsync(command, cancellationToken);
            return Ok(result);
        }
        catch (PedidoNaoEncontradoException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("{numeroPedido}")]
    public async Task<IActionResult> Deletar(string numeroPedido, CancellationToken cancellationToken)
    {
        try
        {
            await _deletarHandler.HandleAsync(new DeletarPedidoCommand(numeroPedido), cancellationToken);
            return NoContent();
        }
        catch (PedidoNaoEncontradoException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}
