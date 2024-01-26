using CadPessoa.Api.Domain.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CadPessoa.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<PessoaFisica> PessoasFisica { get; set; } 
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Endereco>()
                .HasOne(e => e.PessoaFisica)
                .WithMany(p => p.Enderecos)
                .HasForeignKey(e => e.PessoaFisicaId);

            modelBuilder.Entity<Contato>()
                .HasOne(c => c.PessoaFisica)
                .WithMany(p => p.Contatos)
                .HasForeignKey(c => c.PessoaFisicaId);
        }
    }
}
