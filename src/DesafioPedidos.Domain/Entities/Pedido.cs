namespace DesafioPedidos.Domain.Entities;

public class Pedido
{
    public int Id { get; private set; }
    public string NumeroPedido { get; private set; }
    public IReadOnlyCollection<Item> Itens => _itens.AsReadOnly();

    private readonly List<Item> _itens;

    private Pedido()
    {
        NumeroPedido = string.Empty;
        _itens = new List<Item>();
    }

    public Pedido(string numeroPedido, List<Item> itens)
    {
        if (string.IsNullOrWhiteSpace(numeroPedido))
            throw new ArgumentException("Número do pedido não pode ser vazio.", nameof(numeroPedido));
        if (itens == null || itens.Count == 0)
            throw new ArgumentException("Pedido deve ter ao menos um item.", nameof(itens));

        NumeroPedido = numeroPedido;
        _itens = itens;
    }

    //Valor total = soma de (precoUnitario * qtd) de cada item
    public decimal ValorTotal => _itens.Sum(i => i.PrecoUnitario * i.Qtd);

    //Quantidade total de itens = soma das qtd de cada item
    public int QuantidadeTotalItens => _itens.Sum(i => i.Qtd);

    public void AtualizarItens(List<Item> novosItens)
    {
        if (novosItens == null || novosItens.Count == 0)
            throw new ArgumentException("Pedido deve ter ao menos um item.", nameof(novosItens));

        _itens.Clear();
        _itens.AddRange(novosItens);
    }
}
