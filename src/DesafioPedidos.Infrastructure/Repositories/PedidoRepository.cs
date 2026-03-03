using DesafioPedidos.Domain.Entities;
using DesafioPedidos.Domain.Interfaces.Repositories;
using DesafioPedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioPedidos.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pedido?> BuscarPorNumeroAsync(string numeroPedido, CancellationToken cancellationToken = default)
        => await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.NumeroPedido == numeroPedido, cancellationToken);

    public async Task<IEnumerable<Pedido>> ListarTodosAsync(CancellationToken cancellationToken = default)
        => await _context.Pedidos
            .Include(p => p.Itens)
            .ToListAsync(cancellationToken);

    public async Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        await _context.Pedidos.AddAsync(pedido, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExisteAsync(string numeroPedido, CancellationToken cancellationToken = default)
        => await _context.Pedidos.AnyAsync(p => p.NumeroPedido == numeroPedido, cancellationToken);
}
