using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CaseItau.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TIPO_FUNDO",
                columns: table => new
                {
                    CODIGO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NOME = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_FUNDO", x => x.CODIGO);
                });

            migrationBuilder.CreateTable(
                name: "FUNDO",
                columns: table => new
                {
                    CODIGO = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NOME = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CNPJ = table.Column<string>(type: "TEXT", maxLength: 18, nullable: false),
                    CODIGO_TIPO = table.Column<int>(type: "INTEGER", nullable: false),
                    PATRIMONIO = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNDO", x => x.CODIGO);
                    table.ForeignKey(
                        name: "FK_FUNDO_TIPO_FUNDO_CODIGO_TIPO",
                        column: x => x.CODIGO_TIPO,
                        principalTable: "TIPO_FUNDO",
                        principalColumn: "CODIGO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TIPO_FUNDO",
                columns: new[] { "CODIGO", "NOME" },
                values: new object[,]
                {
                    { 1, "Fundo de Renda Fixa" },
                    { 2, "Fundo de Ações" },
                    { 3, "Fundo Multimercado" },
                    { 4, "Fundo de Câmbio" },
                    { 5, "Fundo de Previdência" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FUNDO_CODIGO_TIPO",
                table: "FUNDO",
                column: "CODIGO_TIPO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FUNDO");

            migrationBuilder.DropTable(
                name: "TIPO_FUNDO");
        }
    }
}
