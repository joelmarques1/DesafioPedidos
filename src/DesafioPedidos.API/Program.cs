using DesafioPedidos.Application.Commands.AtualizarPedido;
using DesafioPedidos.Application.Commands.CriarPedido;
using DesafioPedidos.Application.Commands.DeletarPedido;
using DesafioPedidos.Application.Queries.BuscarPedido;
using DesafioPedidos.Application.Queries.ListarPedidos;
using DesafioPedidos.Application.Services;
using DesafioPedidos.Application.StatusRules;
using DesafioPedidos.Domain.Interfaces.Repositories;
using DesafioPedidos.Infrastructure.Data;
using DesafioPedidos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Banco de Dados em Memoria ─────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PedidosDb"));

// ── Repositorios (Dependency Inversion) ───────────────────────────────────
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// ── Handlers CQRS ─────────────────────────────────────────────────────────
builder.Services.AddScoped<ICriarPedidoHandler, CriarPedidoHandler>();
builder.Services.AddScoped<IAtualizarPedidoHandler, AtualizarPedidoHandler>();
builder.Services.AddScoped<IDeletarPedidoHandler, DeletarPedidoHandler>();
builder.Services.AddScoped<IBuscarPedidoHandler, BuscarPedidoHandler>();
builder.Services.AddScoped<IListarPedidosHandler, ListarPedidosHandler>();

// ── Status Rules - cada regra registada separadamente (Open/Closed) ────────
builder.Services.AddScoped<IStatusRule, ReprovadoRule>();
builder.Services.AddScoped<IStatusRule, AprovadoRule>();
builder.Services.AddScoped<IStatusRule, AprovadoValorMenorRule>();
builder.Services.AddScoped<IStatusRule, AprovadoValorMaiorRule>();
builder.Services.AddScoped<IStatusRule, AprovadoQtdMenorRule>();
builder.Services.AddScoped<IStatusRule, AprovadoQtdMaiorRule>();

// ── Servico de Status ──────────────────────────────────────────────────────
builder.Services.AddScoped<IStatusService, StatusService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

// Necessario para os testes de integracao
public partial class Program { }
