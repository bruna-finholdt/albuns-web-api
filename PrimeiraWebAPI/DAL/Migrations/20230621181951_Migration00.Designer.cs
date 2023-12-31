﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PrimeiraWebAPI.DAL;

#nullable disable

namespace PrimeiraWebAPI.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230621181951_Migration00")]
    partial class Migration00
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
#pragma warning restore 612, 618
        }
    }
}
