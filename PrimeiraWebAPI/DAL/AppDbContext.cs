using Microsoft.EntityFrameworkCore;
using PrimeiraWebAPI.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraWebAPI.DAL
{
    public class AppDbContext : DbContext //Esta é a classe que vai gerenciar nosso banco de dados.
                                          //Nela vamos indicar todas as entidades que devem ser mapeadas
                                          //para tabelas do banco.
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public virtual DbSet<Album>? Albuns { get; set; }

        public virtual DbSet<Avaliacao>? Avaliacoes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(x => x.Nome).IsUnicode(false);
                entity.Property(x => x.Artista).IsUnicode(false);
            });

            modelBuilder.Entity<Avaliacao>(entity =>
            {
                entity.Property(x => x.Comentario).IsUnicode(false);
                entity.HasOne(x => x.Album) // entidade virtual que indica relação
                .WithMany(y => y.Avaliacoes) // coleção na entidade Album
                .HasForeignKey(x => x.IdAlbum) //IdAlbum da entidade avaliacao
                .OnDelete(DeleteBehavior.Cascade); // cascade delete - quando um album for removido, suas avaliações relacionadas também serão
            });
        }
    }
}

