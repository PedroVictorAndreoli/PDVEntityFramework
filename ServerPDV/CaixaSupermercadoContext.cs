using Microsoft.EntityFrameworkCore;
using ServerPDV.Models;

namespace ServerPDV
{
    public class CaixaSupermercadoContext : DbContext
    {
        public CaixaSupermercadoContext(DbContextOptions<CaixaSupermercadoContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Item> item { get; set; }

        public DbSet<Item> cupom { get; set; }

        public DbSet<CupomItem> itensCupom { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=CaixaSupermercado;Integrated Security=True;");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.UId);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Login).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Senha).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descricao).IsRequired();
                entity.Property(e => e.Unidade).IsRequired();
                entity.Property(e => e.PrecoUnit).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.EstoqueInterno).IsRequired();
                entity.Property(e => e.EstoqueGondola).IsRequired();
            });

            modelBuilder.Entity<Cupom>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DtEmissao).IsRequired();
                entity.Property(e => e.TotalVenda).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.CPF).HasMaxLength(11);
            });

            modelBuilder.Entity<CupomItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Qtde).IsRequired();
                entity.Property(e => e.PrecoUnit).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalItem).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Uid).IsRequired();

                entity.HasOne<Cupom>()
                    .WithMany()
                    .HasForeignKey(e => e.cupomID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Item>()
                    .WithMany()
                    .HasForeignKey(e => e.itemID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.Uid)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
