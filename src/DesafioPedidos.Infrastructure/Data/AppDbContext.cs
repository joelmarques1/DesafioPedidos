using DesafioPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioPedidos.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Item> Itens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.NumeroPedido).IsRequired().HasMaxLength(100);
            entity.HasIndex(p => p.NumeroPedido).IsUnique();

            entity.HasMany(p => p.Itens)
                  .WithOne()
                  .HasForeignKey("PedidoId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Descricao).IsRequired().HasMaxLength(200);
            entity.Property(i => i.PrecoUnitario).IsRequired();
        });
    }
}
