using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeiraWebAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Migration00 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albuns",
                columns: table => new
                {
                    IdAlbum = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Artista = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Anolancamento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albuns", x => x.IdAlbum);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Albuns");
        }
    }
}
