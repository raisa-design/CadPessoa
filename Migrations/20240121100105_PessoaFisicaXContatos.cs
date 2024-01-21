using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadPessoa.Api.Migrations
{
    public partial class PessoaFisicaXContatos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefoneOuEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoContato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PessoaFisicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contatos_PessoaFisicas_PessoaFisicaId",
                        column: x => x.PessoaFisicaId,
                        principalTable: "pessoaFisicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_PessoaFisicaId",
                table: "Contatos",
                column: "PessoaFisicaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contatos");
        }
    }
}
