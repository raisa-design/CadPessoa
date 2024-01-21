namespace CadPessoa.Api.Domain.Entidades
{
    public class Contato
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string TelefoneOuEmail { get; set; }
        public string TipoContato { get; set; }
        public Guid PessoaFisicaId { get; set; }
        public PessoaFisica? PessoaFisica { get; set; }
    }
}
