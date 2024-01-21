using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadPessoa.Api.Migrations
{
    public partial class PessoaFisicaXContatosIITEST : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_pessoaFisicas_PessoaFisicaId",
                table: "Contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_Enderecos_pessoaFisicas_PessoaFisicaId",
                table: "Enderecos");

            migrationBuilder.DropIndex(
                name: "IX_Enderecos_PessoaFisicaId",
                table: "Enderecos");

            migrationBuilder.DropIndex(
                name: "IX_Contatos_PessoaFisicaId",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "PessoaFisicaId",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "PessoaFisicaId",
                table: "Contatos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PessoaFisicaId",
                table: "Enderecos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PessoaFisicaId",
                table: "Contatos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_PessoaFisicaId",
                table: "Enderecos",
                column: "PessoaFisicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_PessoaFisicaId",
                table: "Contatos",
                column: "PessoaFisicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_pessoaFisicas_PessoaFisicaId",
                table: "Contatos",
                column: "PessoaFisicaId",
                principalTable: "pessoaFisicas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enderecos_pessoaFisicas_PessoaFisicaId",
                table: "Enderecos",
                column: "PessoaFisicaId",
                principalTable: "pessoaFisicas",
                principalColumn: "Id");
        }
    }
}
