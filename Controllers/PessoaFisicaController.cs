using CadPessoa.Api.Data;
using CadPessoa.Api.Domain.Entidades;
using CadPessoa.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadPessoa.Api.Controllers
{
    [Route("api/pessoa-fisica")]
    public class PessoaFisicaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PessoaFisicaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaFisica>>> GetAnuncios()
        {
            if (_context.PessoasFisica == null)
            {
                return NotFound();
            }
            return await _context.PessoasFisica
                .Include(p => p.Enderecos)
                .Include(p => p.Contatos)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaFisica>> GetPessoaFisica(Guid id)
        {
            var pessoaFisica = await _context.PessoasFisica.Include(x => x.Contatos).Include(x => x.Enderecos).FirstOrDefaultAsync(x => x.Id == id);

            if (pessoaFisica == null)
            {
                return NotFound();
            }

            return Ok(pessoaFisica);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoaFisica(Guid id, [FromBody] PessoaFisicaEditViewModel pessoaFisicaViewModel)
        {
            var pessoaFisica = await _context.PessoasFisica.Where(x => x.Id == id).Include(x => x.Contatos).Include(x => x.Enderecos).FirstAsync();
            if (pessoaFisica == null)
            {
                return NotFound();
            }

            pessoaFisica.Nome = pessoaFisicaViewModel.Nome;
            pessoaFisica.SobreNome = pessoaFisicaViewModel.SobreNome;
            pessoaFisica.DataNascimento = pessoaFisicaViewModel.DataNascimento;
            pessoaFisica.Email = pessoaFisicaViewModel.Email;
            pessoaFisica.Cpf = pessoaFisicaViewModel.Cpf;
            pessoaFisica.Rg = pessoaFisicaViewModel.Rg;

            pessoaFisica.Enderecos.Clear();
            pessoaFisica.Contatos.Clear();

            if (pessoaFisica.Enderecos != null)
            {
                foreach (var enderecoViewModel in pessoaFisicaViewModel.Enderecos)
                {
                    var endereco = new Endereco
                    {
                        Logradouro = enderecoViewModel.Logradouro,
                        Complemento = enderecoViewModel.Complemento,
                        Numero = enderecoViewModel.Numero,
                        Cidade = enderecoViewModel.Cidade,
                        Cep = enderecoViewModel.Cep,
                        Estado = enderecoViewModel.Estado
                    };
                    pessoaFisica.Enderecos.Add(endereco);
                }
            }

            if (pessoaFisica.Contatos != null)
            {

                foreach (var contatoViewModel in pessoaFisicaViewModel.Contatos)
                {
                    var contato = new Contato
                    {
                        Nome = contatoViewModel.Nome,
                        TelefoneOuEmail = contatoViewModel.TelefoneOuEmail,
                        TipoContato = contatoViewModel.TipoContato
                    };
                    pessoaFisica.Contatos.Add(contato);
                }
            }

            try
            {
                _context.PessoasFisica.Update(pessoaFisica);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PessoasFisica.Any(e => e.Id == pessoaFisicaViewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(pessoaFisica);
        }


        [HttpPost]
        public async Task<ActionResult<PessoaFisica>> PostAnuncio([FromBody] PessoaFisicaViewModel pessoaFisicaViewModel)
        {
            if (_context.PessoasFisica == null)
            {
                return Problem("Entity set 'DataContext.Anuncios'  is null.");
            }

            var pessoaFisica = new PessoaFisica
            {
                Id = Guid.NewGuid(),
                Nome = pessoaFisicaViewModel.Nome,
                SobreNome = pessoaFisicaViewModel.SobreNome,
                DataNascimento = pessoaFisicaViewModel.DataNascimento,
                Email = pessoaFisicaViewModel.Email,
                Cpf = pessoaFisicaViewModel.Cpf,
                Rg = pessoaFisicaViewModel.Rg,
            };

       
            if (pessoaFisica.Enderecos != null)
            {
                pessoaFisica.Enderecos = pessoaFisicaViewModel.Enderecos.Select(e => new Endereco
                {
                    Id = Guid.NewGuid(),
                    Logradouro = e.Logradouro,
                    Complemento = e.Complemento,
                    Numero = e.Numero,
                    Cidade = e.Cidade,
                    Cep = e.Cep,
                    Estado = e.Estado,
                    PessoaFisicaId = pessoaFisica.Id
                }).ToList();
            }

            if (pessoaFisica.Contatos != null)
            {
                pessoaFisica.Contatos = pessoaFisicaViewModel.Contatos.Select(c => new Contato
                {
                    Id = Guid.NewGuid(),
                    Nome = c.Nome,
                    TelefoneOuEmail = c.TelefoneOuEmail,
                    TipoContato = c.TipoContato,
                    PessoaFisicaId = pessoaFisica.Id
                }).ToList();
            }

                _context.PessoasFisica.Add(pessoaFisica);
            await _context.SaveChangesAsync();

            return Ok(pessoaFisica);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PessoaFisica>> DeletePessoaFisica(Guid id)
        {
            var pessoaFisica = await _context.PessoasFisica.Include(x => x.Contatos).Include(x => x.Enderecos).FirstOrDefaultAsync(x => x.Id == id);
            if (pessoaFisica == null)
            {
                return NotFound();
            }

            _context.PessoasFisica.Remove(pessoaFisica);
            await _context.SaveChangesAsync();

            return Ok(pessoaFisica);
        }


    }
}
