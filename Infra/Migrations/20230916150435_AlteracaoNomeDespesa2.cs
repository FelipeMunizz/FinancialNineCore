using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoNomeDespesa2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Despesa_Categoria_IdCategoria",
                table: "Despesa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Despesa",
                table: "Despesa");

            migrationBuilder.RenameTable(
                name: "Despesa",
                newName: "Lancamento");

            migrationBuilder.RenameIndex(
                name: "IX_Despesa_IdCategoria",
                table: "Lancamento",
                newName: "IX_Lancamento_IdCategoria");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lancamento",
                table: "Lancamento",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamento_Categoria_IdCategoria",
                table: "Lancamento",
                column: "IdCategoria",
                principalTable: "Categoria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamento_Categoria_IdCategoria",
                table: "Lancamento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lancamento",
                table: "Lancamento");

            migrationBuilder.RenameTable(
                name: "Lancamento",
                newName: "Despesa");

            migrationBuilder.RenameIndex(
                name: "IX_Lancamento_IdCategoria",
                table: "Despesa",
                newName: "IX_Despesa_IdCategoria");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Despesa",
                table: "Despesa",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Despesa_Categoria_IdCategoria",
                table: "Despesa",
                column: "IdCategoria",
                principalTable: "Categoria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
