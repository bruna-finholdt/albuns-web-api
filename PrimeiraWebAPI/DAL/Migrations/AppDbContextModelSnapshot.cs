﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PrimeiraWebAPI.DAL;

#nullable disable

namespace PrimeiraWebAPI.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PrimeiraWebAPI.Domain.Entity.Album", b =>
                {
                    b.Property<int>("IdAlbum")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAlbum"));

                    b.Property<int>("Anolancamento")
                        .HasColumnType("int");

                    b.Property<string>("Artista")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Nome")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.HasKey("IdAlbum");

                    b.ToTable("Albuns");
                });

            modelBuilder.Entity("PrimeiraWebAPI.Domain.Entity.Avaliacao", b =>
                {
                    b.Property<int>("IdAvaliacao")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAvaliacao"));

                    b.Property<string>("Comentario")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("IdAlbum")
                        .HasColumnType("int");

                    b.Property<int>("Nota")
                        .HasColumnType("int");

                    b.HasKey("IdAvaliacao");

                    b.HasIndex("IdAlbum");

                    b.ToTable("Avaliacoes");
                });

            modelBuilder.Entity("PrimeiraWebAPI.Domain.Entity.Avaliacao", b =>
                {
                    b.HasOne("PrimeiraWebAPI.Domain.Entity.Album", "Album")
                        .WithMany("Avaliacoes")
                        .HasForeignKey("IdAlbum")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");
                });

            modelBuilder.Entity("PrimeiraWebAPI.Domain.Entity.Album", b =>
                {
                    b.Navigation("Avaliacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
