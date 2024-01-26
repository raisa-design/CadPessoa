using Microsoft.AspNetCore.Identity;

namespace CadPessoa.Api.Domain.Entidades
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public IdentityUser user { get; set; }
        public string UrlImagem { get; set; }
        public string Telefone { get; set; }
    }
}
