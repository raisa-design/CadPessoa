namespace CadPessoa.Api.Models
{
    public class PessoaFisicaEditViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public List<EnderecoEditViewModel> Enderecos { get; set; }
        public List<ContatoEditViewModel> Contatos { get; set; }
    }

    public class EnderecoEditViewModel
    {
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public int Numero { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Estado { get; set; }
    }

    public class ContatoEditViewModel
    {

        public string Nome { get; set; }
        public string TelefoneOuEmail { get; set; }
        public string TipoContato { get; set; }
    }
}
