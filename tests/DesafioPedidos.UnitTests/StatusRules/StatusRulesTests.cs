using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Application.StatusRules;
using DesafioPedidos.Domain.Entities;
using Xunit;

namespace DesafioPedidos.UnitTests.StatusRules;

public class StatusRulesTests
{
    // Pedido base: valor total = (10*1) + (5*2) = 20, qtd total = 3
    private static Pedido CriarPedidoBase() => new("123456", new List<Item>
    {
        new("Item A", 10, 1),
        new("Item B", 5, 2)
    });

    [Fact]
    public void ReprovadoRule_DeveAplicar_QuandoStatusReprovado()
    {
        var rule = new ReprovadoRule();
        var request = new StatusRequestDto { Status = "REPROVADO", Pedido = "123456" };
        Assert.True(rule.Applies(request, CriarPedidoBase()));
        Assert.Equal("REPROVADO", rule.StatusGerado);
    }

    [Fact]
    public void ReprovadoRule_NaoDeveAplicar_QuandoStatusAprovado()
    {
        var rule = new ReprovadoRule();
        var request = new StatusRequestDto { Status = "APROVADO", Pedido = "123456" };
        Assert.False(rule.Applies(request, CriarPedidoBase()));
    }

    [Fact]
    public void AprovadoRule_DeveAplicar_QuandoValorEQtdCorretos()
    {
        var rule = new AprovadoRule();
        var request = new StatusRequestDto { Status = "APROVADO", ValorAprovado = 20, ItensAprovados = 3, Pedido = "123456" };
        Assert.True(rule.Applies(request, CriarPedidoBase()));
        Assert.Equal("APROVADO", rule.StatusGerado);
    }

    [Fact]
    public void AprovadoValorMenorRule_DeveAplicar_QuandoValorMenorQueTotal()
    {
        var rule = new AprovadoValorMenorRule();
        var request = new StatusRequestDto { Status = "APROVADO", ValorAprovado = 10, ItensAprovados = 3, Pedido = "123456" };
        Assert.True(rule.Applies(request, CriarPedidoBase()));
        Assert.Equal("APROVADO_VALOR_A_MENOR", rule.StatusGerado);
    }

    [Fact]
    public void AprovadoValorMaiorRule_DeveAplicar_QuandoValorMaiorQueTotal()
    {
        var rule = new AprovadoValorMaiorRule();
        var request = new StatusRequestDto { Status = "APROVADO", ValorAprovado = 21, ItensAprovados = 3, Pedido = "123456" };
        Assert.True(rule.Applies(request, CriarPedidoBase()));
        Assert.Equal("APROVADO_VALOR_A_MAIOR", rule.StatusGerado);
    }

    [Fact]
    public void AprovadoQtdMenorRule_DeveAplicar_QuandoQtdMenorQueTotal()
    {
        var rule = new AprovadoQtdMenorRule();
        var request = new StatusRequestDto { Status = "APROVADO", ValorAprovado = 20, ItensAprovados = 2, Pedido = "123456" };
        Assert.True(rule.Applies(request, CriarPedidoBase()));
        Assert.Equal("APROVADO_QTD_A_MENOR", rule.StatusGerado);
    }

    [Fact]
    public void AprovadoQtdMaiorRule_DeveAplicar_QuandoQtdMaiorQueTotal()
    {
        var rule = new AprovadoQtdMaiorRule();
        var request = new StatusRequestDto { Status = "APROVADO", ValorAprovado = 20, ItensAprovados = 4, Pedido = "123456" };
        Assert.True(rule.Applies(request, CriarPedidoBase()));
        Assert.Equal("APROVADO_QTD_A_MAIOR", rule.StatusGerado);
    }

    [Fact]
    public void MultiplaRegras_Exemplo3_DevemRetornarValorMaiorEQtdMaior()
    {
        // Exemplo #3 do desafio: itensAprovados=4, valorAprovado=21
        var rules = new List<IStatusRule>
        {
            new ReprovadoRule(),
            new AprovadoRule(),
            new AprovadoValorMenorRule(),
            new AprovadoValorMaiorRule(),
            new AprovadoQtdMenorRule(),
            new AprovadoQtdMaiorRule()
        };

        var request = new StatusRequestDto { Status = "APROVADO", ValorAprovado = 21, ItensAprovados = 4, Pedido = "123456" };
        var pedido = CriarPedidoBase();

        var resultado = rules
            .Where(r => r.Applies(request, pedido))
            .Select(r => r.StatusGerado)
            .ToList();

        Assert.Contains("APROVADO_VALOR_A_MAIOR", resultado);
        Assert.Contains("APROVADO_QTD_A_MAIOR", resultado);
        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public void Pedido_ValorTotal_DeveSerCalculadoCorretamente()
    {
        var pedido = CriarPedidoBase();
        Assert.Equal(20, pedido.ValorTotal);      // (10*1) + (5*2) = 20
        Assert.Equal(3, pedido.QuantidadeTotalItens); // 1 + 2 = 3
    }
}
