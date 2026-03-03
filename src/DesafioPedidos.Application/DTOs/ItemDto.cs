using System.ComponentModel.DataAnnotations;

namespace DesafioPedidos.Application.DTOs;

public class ItemDto
{
    [Required(ErrorMessage = "Descrição é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Preço unitário deve ser maior que zero.")]
    public decimal PrecoUnitario { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero.")]
    public int Qtd { get; set; }
}
