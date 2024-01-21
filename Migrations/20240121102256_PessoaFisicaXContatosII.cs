using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadPessoa.Api.Migrations
{
    public partial class PessoaFisicaXContatosII : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_pessoaFisicas_PessoaFisicaId",
                table: "Contatos");

            migrationBuilder.AlterColumn<Guid>(
                name: "PessoaFisicaId",
                table: "Contatos",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_pessoaFisicas_PessoaFisicaId",
                table: "Contatos",
                column: "PessoaFisicaId",
                principalTable: "pessoaFisicas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_pessoaFisicas_PessoaFisicaId",
                table: "Contatos");

            migrationBuilder.AlterColumn<Guid>(
                name: "PessoaFisicaId",
                table: "Contatos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_pessoaFisicas_PessoaFisicaId",
                table: "Contatos",
                column: "PessoaFisicaId",
                principalTable: "pessoaFisicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
