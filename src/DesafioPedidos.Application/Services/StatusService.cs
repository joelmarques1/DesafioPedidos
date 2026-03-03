using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Application.StatusRules;
using DesafioPedidos.Domain.Interfaces.Repositories;

namespace DesafioPedidos.Application.Services;

public class StatusService : IStatusService
{
    private readonly IPedidoRepository _repository;
    private readonly IEnumerable<IStatusRule> _rules;

    public StatusService(IPedidoRepository repository, IEnumerable<IStatusRule> rules)
    {
        _repository = repository;
        _rules = rules;
    }

    public async Task<StatusResponseDto> ProcessarAsync(StatusRequestDto request, CancellationToken cancellationToken = default)
    {
        var pedido = await _repository.BuscarPorNumeroAsync(request.Pedido, cancellationToken);

        if (pedido is null)
        {
            return new StatusResponseDto
            {
                Pedido = request.Pedido,
                Status = new List<string> { "CODIGO_PEDIDO_INVALIDO" }
            };
        }

        // Aplica TODAS as regras e coleta os status gerados (DRY + Open/Closed)
        var statusGerados = _rules
            .Where(rule => rule.Applies(request, pedido))
            .Select(rule => rule.StatusGerado)
            .ToList();

        return new StatusResponseDto
        {
            Pedido = request.Pedido,
            Status = statusGerados
        };
    }
}
