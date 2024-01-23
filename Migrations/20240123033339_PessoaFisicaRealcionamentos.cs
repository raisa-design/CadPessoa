using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadPessoa.Api.Migrations
{
    public partial class PessoaFisicaRealcionamentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contato_PessoasFisica_PessoaFisicaId",
                table: "Contato");

            migrationBuilder.DropForeignKey(
                name: "FK_Endereco_PessoasFisica_PessoaFisicaId",
                table: "Endereco");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Endereco",
                table: "Endereco");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contato",
                table: "Contato");

            migrationBuilder.RenameTable(
                name: "Endereco",
                newName: "Enderecos");

            migrationBuilder.RenameTable(
                name: "Contato",
                newName: "Contatos");

            migrationBuilder.RenameIndex(
                name: "IX_Endereco_PessoaFisicaId",
                table: "Enderecos",
                newName: "IX_Enderecos_PessoaFisicaId");

            migrationBuilder.RenameIndex(
                name: "IX_Contato_PessoaFisicaId",
                table: "Contatos",
                newName: "IX_Contatos_PessoaFisicaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enderecos",
                table: "Enderecos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contatos",
                table: "Contatos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_PessoasFisica_PessoaFisicaId",
                table: "Contatos",
                column: "PessoaFisicaId",
                principalTable: "PessoasFisica",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enderecos_PessoasFisica_PessoaFisicaId",
                table: "Enderecos",
                column: "PessoaFisicaId",
                principalTable: "PessoasFisica",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_PessoasFisica_PessoaFisicaId",
                table: "Contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_Enderecos_PessoasFisica_PessoaFisicaId",
                table: "Enderecos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enderecos",
                table: "Enderecos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contatos",
                table: "Contatos");

            migrationBuilder.RenameTable(
                name: "Enderecos",
                newName: "Endereco");

            migrationBuilder.RenameTable(
                name: "Contatos",
                newName: "Contato");

            migrationBuilder.RenameIndex(
                name: "IX_Enderecos_PessoaFisicaId",
                table: "Endereco",
                newName: "IX_Endereco_PessoaFisicaId");

            migrationBuilder.RenameIndex(
                name: "IX_Contatos_PessoaFisicaId",
                table: "Contato",
                newName: "IX_Contato_PessoaFisicaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Endereco",
                table: "Endereco",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contato",
                table: "Contato",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contato_PessoasFisica_PessoaFisicaId",
                table: "Contato",
                column: "PessoaFisicaId",
                principalTable: "PessoasFisica",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Endereco_PessoasFisica_PessoaFisicaId",
                table: "Endereco",
                column: "PessoaFisicaId",
                principalTable: "PessoasFisica",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
