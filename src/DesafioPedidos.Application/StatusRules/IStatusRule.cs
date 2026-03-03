using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Entities;

namespace DesafioPedidos.Application.StatusRules;

/// <summary>
/// Contrato para cada regra de status.
/// Para adicionar nova regra: crie nova classe, NAO altere as existentes. (Open/Closed)
/// </summary>
public interface IStatusRule
{
    string StatusGerado { get; }
    bool Applies(StatusRequestDto request, Pedido pedido);
}
