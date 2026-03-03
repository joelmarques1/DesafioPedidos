namespace DesafioPedidos.Application.DTOs;

public class StatusResponseDto
{
    public string Pedido { get; set; } = string.Empty;
    public List<string> Status { get; set; } = new();
}
