using DesafioPedidos.Application.Commands.CriarPedido;
using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Exceptions;
using DesafioPedidos.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace DesafioPedidos.UnitTests.Handlers;

public class CriarPedidoHandlerTests
{
    private readonly Mock<IPedidoRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_DeveCriarPedido_QuandoDadosValidos()
    {
        _repositoryMock.Setup(r => r.ExisteAsync("123456", default)).ReturnsAsync(false);

        var handler = new CriarPedidoHandler(_repositoryMock.Object);
        var command = new CriarPedidoCommand("123456", new List<ItemDto>
        {
            new() { Descricao = "Item A", PrecoUnitario = 10, Qtd = 1 },
            new() { Descricao = "Item B", PrecoUnitario = 5, Qtd = 2 }
        });

        var result = await handler.HandleAsync(command);

        Assert.Equal("123456", result.Pedido);
        Assert.Equal(20, result.ValorTotal);
        Assert.Equal(3, result.QuantidadeTotalItens);
        Assert.Equal(2, result.Itens.Count);
        _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Domain.Entities.Pedido>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoPedidoDuplicado()
    {
        _repositoryMock.Setup(r => r.ExisteAsync("123456", default)).ReturnsAsync(true);

        var handler = new CriarPedidoHandler(_repositoryMock.Object);
        var command = new CriarPedidoCommand("123456", new List<ItemDto>
        {
            new() { Descricao = "Item A", PrecoUnitario = 10, Qtd = 1 }
        });

        await Assert.ThrowsAsync<PedidoDuplicadoException>(() => handler.HandleAsync(command));
        _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Domain.Entities.Pedido>(), default), Times.Never);
    }
}
