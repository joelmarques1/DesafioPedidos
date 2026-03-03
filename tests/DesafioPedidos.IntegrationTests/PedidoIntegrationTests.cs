using System.Net;
using System.Net.Http.Json;
using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DesafioPedidos.IntegrationTests;

public class PedidoIntegrationTests
{
    // Método auxiliar para criar a Factory com Banco Isolado
    private static WebApplicationFactory<Program> CriarFactory(string dbName)
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 1. Remove a configuração original do DbContext (SQL Server)
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null) services.Remove(descriptor);

                    // 2. Adiciona o Banco em Memória com o nome fixo para este teste
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase(dbName));

                    // 3. Garante que o banco seja criado
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();
                });
            });
    }

    // Método auxiliar para popular o banco via API
    private static async Task CriarPedidoBase(HttpClient client, string numeroPedido)
    {
        var request = new PedidoRequestDto
        {
            Pedido = numeroPedido,
            Itens = new List<ItemDto>
            {
                new() { Descricao = "Item A", PrecoUnitario = 10, Qtd = 1 }, // Total 10
                new() { Descricao = "Item B", PrecoUnitario = 5, Qtd = 2 }   // Total 10 (Total 20)
            }
        };
        var response = await client.PostAsJsonAsync("/api/pedido", request);

        // Se falhar aqui, Controller de Pedido não está retornando 201 Created
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostPedido_DeveRetornar201_QuandoPedidoValido()
    {
        var dbName = Guid.NewGuid().ToString(); // Nome único para este teste
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        var request = new PedidoRequestDto
        {
            Pedido = "123456",
            Itens = new List<ItemDto>
            {
                new() { Descricao = "Item A", PrecoUnitario = 10, Qtd = 1 },
                new() { Descricao = "Item B", PrecoUnitario = 5, Qtd = 2 }
            }
        };

        var response = await client.PostAsJsonAsync("/api/pedido", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PedidoResponseDto>();
        Assert.Equal("123456", result!.Pedido);
        Assert.Equal(2, result.Itens.Count);
    }

    [Fact]
    public async Task PostPedido_DeveRetornar409_QuandoPedidoDuplicado()
    {
        var dbName = Guid.NewGuid().ToString();
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        var request = new PedidoRequestDto
        {
            Pedido = "DUPLO",
            Itens = new List<ItemDto> { new() { Descricao = "Item", PrecoUnitario = 5, Qtd = 1 } }
        };

        // Primeiro Post (Sucesso)
        await client.PostAsJsonAsync("/api/pedido", request);

        // Segundo Post (Deve falhar com Conflict)
        var response = await client.PostAsJsonAsync("/api/pedido", request);

        // IMPORTANTE: Se falhar aqui dizendo que veio 'Created',é preciso implementar a validação no Controller!
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task GetPedido_DeveRetornar404_QuandoPedidoNaoExiste()
    {
        var dbName = Guid.NewGuid().ToString();
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/pedido/INEXISTENTE");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeletePedido_DeveRetornar204_QuandoPedidoExiste()
    {
        var dbName = Guid.NewGuid().ToString();
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        await CriarPedidoBase(client, "DEL001");

        var response = await client.DeleteAsync("/api/pedido/DEL001");

        // Se der NotFound aqui, é porque o CriarPedidoBase não persistiu os dados (Falta SaveChanges no repo)
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task PostStatus_DeveRetornarCodigoPedidoInvalido_QuandoPedidoNaoExiste()
    {
        var dbName = Guid.NewGuid().ToString();
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        var statusRequest = new StatusRequestDto
        {
            Status = "APROVADO",
            ValorAprovado = 20,
            ItensAprovados = 3,
            Pedido = "INEXISTENTE"
        };

        var response = await client.PostAsJsonAsync("/api/status", statusRequest);
        var result = await response.Content.ReadFromJsonAsync<StatusResponseDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("CODIGO_PEDIDO_INVALIDO", result!.Status);
    }

    [Fact]
    public async Task PostStatus_DeveRetornarAprovado_Exemplo1()
    {
        var dbName = Guid.NewGuid().ToString();
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        await CriarPedidoBase(client, "ST001");

        var statusRequest = new StatusRequestDto
        {
            Status = "APROVADO",
            ValorAprovado = 20,
            ItensAprovados = 3,
            Pedido = "ST001"
        };

        var response = await client.PostAsJsonAsync("/api/status", statusRequest);
        var result = await response.Content.ReadFromJsonAsync<StatusResponseDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("APROVADO", result!.Status);
        Assert.Single(result.Status);
    }

    [Fact]
    public async Task PostStatus_DeveRetornarAprovadoValorMenor_Exemplo2()
    {
        var dbName = Guid.NewGuid().ToString();
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        await CriarPedidoBase(client, "ST002");

        var statusRequest = new StatusRequestDto
        {
            Status = "APROVADO",
            ValorAprovado = 10,
            ItensAprovados = 3,
            Pedido = "ST002"
        };

        var response = await client.PostAsJsonAsync("/api/status", statusRequest);
        var result = await response.Content.ReadFromJsonAsync<StatusResponseDto>();

        Assert.Contains("APROVADO_VALOR_A_MENOR", result!.Status);
    }

    [Fact]
    public async Task PostStatus_DeveRetornarValorMaiorEQtdMaior_Exemplo3()
    {
        var dbName = Guid.NewGuid().ToString();
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        await CriarPedidoBase(client, "ST003");

        var statusRequest = new StatusRequestDto
        {
            Status = "APROVADO",
            ValorAprovado = 21,
            ItensAprovados = 4,
            Pedido = "ST003"
        };

        var response = await client.PostAsJsonAsync("/api/status", statusRequest);
        var result = await response.Content.ReadFromJsonAsync<StatusResponseDto>();

        Assert.Contains("APROVADO_VALOR_A_MAIOR", result!.Status);
        Assert.Contains("APROVADO_QTD_A_MAIOR", result!.Status);
        Assert.Equal(2, result.Status.Count);
    }

    [Fact]
    public async Task PostStatus_DeveRetornarQtdMenor_Exemplo4()
    {
        var dbName = Guid.NewGuid().ToString();
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        await CriarPedidoBase(client, "ST004");

        var statusRequest = new StatusRequestDto
        {
            Status = "APROVADO",
            ValorAprovado = 20,
            ItensAprovados = 2,
            Pedido = "ST004"
        };

        var response = await client.PostAsJsonAsync("/api/status", statusRequest);
        var result = await response.Content.ReadFromJsonAsync<StatusResponseDto>();

        Assert.Contains("APROVADO_QTD_A_MENOR", result!.Status);
    }

    [Fact]
    public async Task PostStatus_DeveRetornarReprovado_Exemplo5()
    {
        var dbName = Guid.NewGuid().ToString();
        using var factory = CriarFactory(dbName);
        var client = factory.CreateClient();

        await CriarPedidoBase(client, "ST005");

        var statusRequest = new StatusRequestDto
        {
            Status = "REPROVADO",
            ValorAprovado = 0,
            ItensAprovados = 0,
            Pedido = "ST005"
        };

        var response = await client.PostAsJsonAsync("/api/status", statusRequest);
        var result = await response.Content.ReadFromJsonAsync<StatusResponseDto>();

        Assert.Contains("REPROVADO", result!.Status);
        Assert.Single(result.Status);
    }
}