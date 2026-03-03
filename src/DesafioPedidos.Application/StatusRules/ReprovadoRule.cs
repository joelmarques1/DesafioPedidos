using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Entities;

namespace DesafioPedidos.Application.StatusRules;

public class ReprovadoRule : IStatusRule
{
    public string StatusGerado => "REPROVADO";

    public bool Applies(StatusRequestDto request, Pedido pedido)
        => request.Status.Equals("REPROVADO", StringComparison.OrdinalIgnoreCase);
}
