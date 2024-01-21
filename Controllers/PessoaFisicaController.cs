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

        [Authorize]
        // GET: api/Anuncios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaFisica>>> GetAnuncios()
        {
            if (_context.pessoaFisicas== null)
            {
                return NotFound();
            }
            return await _context.pessoaFisicas
                .Include(p => p.Enderecos)
                .Include(p => p.Contatos)
                .ToListAsync();
        }

        // GET: api/Anuncios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaFisica>> GetAnuncio([FromBody] Guid id)
        {
            if (_context.pessoaFisicas == null)
            {
                return NotFound();
            }
            var anuncio = await _context.pessoaFisicas
                .FirstOrDefaultAsync();

            if (anuncio == null)
            {
                return NotFound();
            }

            return anuncio;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoaFisica([FromBody] Guid id, [FromBody] PessoaFisicaEditViewModel pessoaFisicaViewModel)
        {

            var pessoaFisica = await _context.pessoaFisicas.FindAsync(id);
            if (pessoaFisica == null)
            {
                return NotFound();
            }

            // Atualiza a PessoaFisica com os dados da ViewModel
            pessoaFisica.Nome = pessoaFisicaViewModel.Nome;
            pessoaFisica.SobreNome = pessoaFisicaViewModel.SobreNome;
            pessoaFisica.DataNascimento = pessoaFisicaViewModel.DataNascimento;
            pessoaFisica.Email = pessoaFisicaViewModel.Email;
            pessoaFisica.Cpf = pessoaFisicaViewModel.Cpf;
            pessoaFisica.Rg = pessoaFisicaViewModel.Rg;

            // Atualiza os Enderecos e Contatos com os dados da ViewModel
            foreach (var enderecoViewModel in pessoaFisicaViewModel.Enderecos)
            {
                var endereco = pessoaFisica.Enderecos.FirstOrDefault(e => e.Id == enderecoViewModel.Id);
                if (endereco != null)
                {
                    endereco.Logradouro = enderecoViewModel.Logradouro;
                    endereco.Componente = enderecoViewModel.Componente;
                    endereco.Numero = enderecoViewModel.Numero;
                    endereco.Cidade = enderecoViewModel.Cidade;
                    endereco.Cep = enderecoViewModel.Cep;
                    endereco.Estado = enderecoViewModel.Estado;
                }
            }

            foreach (var contatoViewModel in pessoaFisicaViewModel.Contatos)
            {
                var contato = pessoaFisica.Contatos.FirstOrDefault(c => c.Id == contatoViewModel.Id);
                if (contato != null)
                {
                    contato.Nome = contatoViewModel.Nome;
                    contato.TelefoneOuEmail = contatoViewModel.TelefoneOuEmail;
                    contato.TipoContato = contatoViewModel.TipoContato;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.pessoaFisicas.Any(e => e.Id == pessoaFisicaViewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Anuncios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<PessoaFisica>> PostAnuncio([FromBody] PessoaFisicaViewModel pessoaFisicaViewModel)
        //{
        //    if (_context.pessoaFisicas == null)
        //    {
        //        return Problem("Entity set 'DataContext.Anuncios'  is null.");
        //    }
        //    _context.pessoaFisicas.Add(pessoaFisica);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAnuncio", new { id = pessoaFisica.Id }, pessoaFisica);
        //}

        [HttpPost]
        public async Task<ActionResult<PessoaFisica>> PostAnuncio([FromBody] PessoaFisicaViewModel pessoaFisicaViewModel)
        {
            if (_context.pessoaFisicas == null)
            {
                return Problem("Entity set 'DataContext.Anuncios'  is null.");
            }

            // Cria uma nova PessoaFisica a partir da ViewModel
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

            // Adiciona os endereços e contatos à PessoaFisica
            pessoaFisica.Enderecos = pessoaFisicaViewModel.Enderecos.Select(e => new Endereco
            {
                Id = Guid.NewGuid(),
                Logradouro = e.Logradouro,
                Componente = e.Componente,
                Numero = e.Numero,
                Cidade = e.Cidade,
                Cep = e.Cep,
                Estado = e.Estado,
                PessoaFisicaId = pessoaFisica.Id
            }).ToList();

            pessoaFisica.Contatos = pessoaFisicaViewModel.Contatos.Select(c => new Contato
            {
                Id = Guid.NewGuid(),
                Nome = c.Nome,
                TelefoneOuEmail = c.TelefoneOuEmail,
                TipoContato = c.TipoContato,
                PessoaFisicaId = pessoaFisica.Id
            }).ToList();

            _context.pessoaFisicas.Add(pessoaFisica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnuncio", new { id = pessoaFisica.Id }, pessoaFisica);
        }

        // DELETE: api/Anuncios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnuncio(Guid id)
        {
            if (_context.pessoaFisicas == null)
            {
                return NotFound();
            }
            var anuncio = await _context.pessoaFisicas.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (anuncio == null)
            {
                return NotFound();
            }

            _context.pessoaFisicas.Remove(anuncio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnuncioExists(Guid id)
        {
            return (_context.pessoaFisicas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
