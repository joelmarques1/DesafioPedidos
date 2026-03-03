using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Entities;

namespace DesafioPedidos.Application.StatusRules;

public class AprovadoQtdMenorRule : IStatusRule
{
    public string StatusGerado => "APROVADO_QTD_A_MENOR";

    public bool Applies(StatusRequestDto request, Pedido pedido)
        => request.Status.Equals("APROVADO", StringComparison.OrdinalIgnoreCase)
        && request.ItensAprovados < pedido.QuantidadeTotalItens;
}
