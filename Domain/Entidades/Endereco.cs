namespace CadPessoa.Api.Domain.Entidades
{
    public class Endereco
    {
        public Guid Id { get; set; }
        public string Logradouro { get; set; }
        public string Componente { get; set; }
        public int Numero { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Estado { get; set; }
        PessoaFisica PessoaFisica { get; set; }
    }
}
