namespace DesafioPedidos.Domain.Entities;

public class Item
{
    public int Id { get; private set; }
    public string Descricao { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public int Qtd { get; private set; }
    public int PedidoId { get; private set; }

    private Item()
    {
        Descricao = string.Empty;
    }

    public Item(string descricao, decimal precoUnitario, int qtd)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição não pode ser vazia.", nameof(descricao));
        if (precoUnitario <= 0)
            throw new ArgumentException("Preço unitário deve ser maior que zero.", nameof(precoUnitario));
        if (qtd <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(qtd));

        Descricao = descricao;
        PrecoUnitario = precoUnitario;
        Qtd = qtd;
    }
}
