using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Entities;

namespace DesafioPedidos.Application.StatusRules;

public class AprovadoValorMaiorRule : IStatusRule
{
    public string StatusGerado => "APROVADO_VALOR_A_MAIOR";

    public bool Applies(StatusRequestDto request, Pedido pedido)
        => request.Status.Equals("APROVADO", StringComparison.OrdinalIgnoreCase)
        && request.ValorAprovado > pedido.ValorTotal;
}
