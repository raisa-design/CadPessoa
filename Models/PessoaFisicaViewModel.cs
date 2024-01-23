namespace CadPessoa.Api.Models
{
    public class PessoaFisicaViewModel
    {
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public List<EnderecoViewModel> Enderecos { get; set; }
        public List<ContatoViewModel> Contatos { get; set; }
    }

    public class EnderecoViewModel
    {
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public int Numero { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Estado { get; set; }
    }

    public class ContatoViewModel
    {
        public string Nome { get; set; }
        public string TelefoneOuEmail { get; set; }
        public string TipoContato { get; set; }
    }

}
