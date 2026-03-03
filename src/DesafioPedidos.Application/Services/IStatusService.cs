using DesafioPedidos.Application.DTOs;

namespace DesafioPedidos.Application.Services;

public interface IStatusService
{
    Task<StatusResponseDto> ProcessarAsync(StatusRequestDto request, CancellationToken cancellationToken = default);
}
