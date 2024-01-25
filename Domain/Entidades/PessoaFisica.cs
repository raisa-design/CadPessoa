namespace CadPessoa.Api.Domain.Entidades
{
    public class PessoaFisica
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public List<Endereco>? Enderecos { get; set; } = new List<Endereco>();
        public List<Contato>? Contatos { get; set; } = new List<Contato>();

    }
}
