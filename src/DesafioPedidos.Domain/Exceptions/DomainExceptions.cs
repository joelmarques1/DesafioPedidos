namespace DesafioPedidos.Domain.Exceptions;

public class PedidoNaoEncontradoException : Exception
{
    public PedidoNaoEncontradoException(string numeroPedido)
        : base($"Pedido '{numeroPedido}' não encontrado.") { }
}

public class PedidoDuplicadoException : Exception
{
    public PedidoDuplicadoException(string numeroPedido)
        : base($"Já existe um pedido com o número '{numeroPedido}'.") { }
}
