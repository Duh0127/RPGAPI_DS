using Microsoft.EntityFrameworkCore.Migrations;

namespace RpgApi.Migrations
{
    public partial class MigracaoPersonagemHabilidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonagemHabilidade_Habilidades_HabilidadeId",
                table: "PersonagemHabilidade");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonagemHabilidade_Personagens_PersonagemId",
                table: "PersonagemHabilidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonagemHabilidade",
                table: "PersonagemHabilidade");

            migrationBuilder.RenameTable(
                name: "PersonagemHabilidade",
                newName: "PersonagemHabilidades");

            migrationBuilder.RenameIndex(
                name: "IX_PersonagemHabilidade_HabilidadeId",
                table: "PersonagemHabilidades",
                newName: "IX_PersonagemHabilidades_HabilidadeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonagemHabilidades",
                table: "PersonagemHabilidades",
                columns: new[] { "PersonagemId", "HabilidadeId" });

            migrationBuilder.UpdateData(
                table: "Habilidades",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Dano", "Nome" },
                values: new object[] { 29, "Bola de fogo" });

            migrationBuilder.UpdateData(
                table: "Habilidades",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Dano", "Nome" },
                values: new object[] { 41, "Portal" });

            migrationBuilder.UpdateData(
                table: "Habilidades",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Dano", "Nome" },
                values: new object[] { 37, "Congelar" });

            migrationBuilder.AddForeignKey(
                name: "FK_PersonagemHabilidades_Habilidades_HabilidadeId",
                table: "PersonagemHabilidades",
                column: "HabilidadeId",
                principalTable: "Habilidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonagemHabilidades_Personagens_PersonagemId",
                table: "PersonagemHabilidades",
                column: "PersonagemId",
                principalTable: "Personagens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonagemHabilidades_Habilidades_HabilidadeId",
                table: "PersonagemHabilidades");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonagemHabilidades_Personagens_PersonagemId",
                table: "PersonagemHabilidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonagemHabilidades",
                table: "PersonagemHabilidades");

            migrationBuilder.RenameTable(
                name: "PersonagemHabilidades",
                newName: "PersonagemHabilidade");

            migrationBuilder.RenameIndex(
                name: "IX_PersonagemHabilidades_HabilidadeId",
                table: "PersonagemHabilidade",
                newName: "IX_PersonagemHabilidade_HabilidadeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonagemHabilidade",
                table: "PersonagemHabilidade",
                columns: new[] { "PersonagemId", "HabilidadeId" });

            migrationBuilder.UpdateData(
                table: "Habilidades",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Dano", "Nome" },
                values: new object[] { 50, "O-dama Rasengan" });

            migrationBuilder.UpdateData(
                table: "Habilidades",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Dano", "Nome" },
                values: new object[] { 23, "Chico Bola, Bola de fogo" });

            migrationBuilder.UpdateData(
                table: "Habilidades",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Dano", "Nome" },
                values: new object[] { 36, "Estilo Madeira, Madeirada na nuca" });

            migrationBuilder.AddForeignKey(
                name: "FK_PersonagemHabilidade_Habilidades_HabilidadeId",
                table: "PersonagemHabilidade",
                column: "HabilidadeId",
                principalTable: "Habilidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonagemHabilidade_Personagens_PersonagemId",
                table: "PersonagemHabilidade",
                column: "PersonagemId",
                principalTable: "Personagens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
