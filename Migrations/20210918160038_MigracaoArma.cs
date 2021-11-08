using Microsoft.EntityFrameworkCore.Migrations;

namespace RpgApi.Migrations
{
    public partial class MigracaoArma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Armas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dano = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Armas",
                columns: new[] { "Id", "Dano", "Nome" },
                values: new object[] { 10, 20, "Espada Lendária" });

            migrationBuilder.InsertData(
                table: "Armas",
                columns: new[] { "Id", "Dano", "Nome" },
                values: new object[] { 11, 40, "Cajado de Rubi" });

            migrationBuilder.InsertData(
                table: "Armas",
                columns: new[] { "Id", "Dano", "Nome" },
                values: new object[] { 12, 70, "Arco Flamejante" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Armas");
        }
    }
}
