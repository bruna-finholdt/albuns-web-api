using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeiraWebAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Migration01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    IdAvaliacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlbum = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.IdAvaliacao);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Albuns_IdAlbum",
                        column: x => x.IdAlbum,
                        principalTable: "Albuns",
                        principalColumn: "IdAlbum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_IdAlbum",
                table: "Avaliacoes",
                column: "IdAlbum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacoes");
        }
    }
}

//Update-Database  -Context AppDbContext
