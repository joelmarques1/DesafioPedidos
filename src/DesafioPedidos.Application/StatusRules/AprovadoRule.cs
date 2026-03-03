using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Entities;

namespace DesafioPedidos.Application.StatusRules;

public class AprovadoRule : IStatusRule
{
    public string StatusGerado => "APROVADO";

    public bool Applies(StatusRequestDto request, Pedido pedido)
        => request.Status.Equals("APROVADO", StringComparison.OrdinalIgnoreCase)
        && request.ValorAprovado == pedido.ValorTotal
        && request.ItensAprovados == pedido.QuantidadeTotalItens;
}
