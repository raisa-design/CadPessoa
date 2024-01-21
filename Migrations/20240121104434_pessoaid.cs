using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadPessoa.Api.Migrations
{
    public partial class pessoaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PessoaFisicaId",
                table: "Enderecos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PessoaFisicaId",
                table: "Contatos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enderecos_pessoaFisicas_PessoaFisicaId",
                table: "Enderecos",
                column: "PessoaFisicaId",
                principalTable: "pessoaFisicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
