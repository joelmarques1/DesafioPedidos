using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Entities;

namespace DesafioPedidos.Application.StatusRules;

public class AprovadoQtdMaiorRule : IStatusRule
{
    public string StatusGerado => "APROVADO_QTD_A_MAIOR";

    public bool Applies(StatusRequestDto request, Pedido pedido)
        => request.Status.Equals("APROVADO", StringComparison.OrdinalIgnoreCase)
        && request.ItensAprovados > pedido.QuantidadeTotalItens;
}
