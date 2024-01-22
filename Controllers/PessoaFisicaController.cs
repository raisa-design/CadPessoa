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

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaFisica>> GetPessoaFisica(Guid id)
        {
          var pessoaFisica = await _context.pessoaFisicas.Include(x => x.Contatos).Include(x => x.Enderecos).FirstOrDefaultAsync(x => x.Id == id);

          if (pessoaFisica == null)
          {
            return NotFound();
          }

          return pessoaFisica;
        }

        [HttpPut()]
        public async Task<IActionResult> PutPessoaFisica([FromBody] PessoaFisicaEditViewModel pessoaFisicaViewModel)
        {
          var pessoaFisica = await _context.pessoaFisicas.Where(x => x.Id == pessoaFisicaViewModel.Id).Include(x => x.Contatos).Include(x => x.Enderecos).FirstAsync();
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

          // Limpa as listas de Enderecos e Contatos
          pessoaFisica.Enderecos.Clear();
          pessoaFisica.Contatos.Clear();

          // Adiciona os novos Enderecos e Contatos da ViewModel
          foreach (var enderecoViewModel in pessoaFisicaViewModel.Enderecos)
          {
            var endereco = new Endereco
            {
              Logradouro = enderecoViewModel.Logradouro,
              Componente = enderecoViewModel.Componente,
              Numero = enderecoViewModel.Numero,
              Cidade = enderecoViewModel.Cidade,
              Cep = enderecoViewModel.Cep,
              Estado = enderecoViewModel.Estado
            };
            pessoaFisica.Enderecos.Add(endereco);
          }

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

          try
          {
            _context.pessoaFisicas.Update(pessoaFisica);
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
        public async Task<IActionResult> DeletePessoaFisica(Guid id)
        {
          var pessoaFisica = await _context.pessoaFisicas.Include(x => x.Contatos).Include(x => x.Enderecos).FirstOrDefaultAsync(x => x.Id == id);
          if (pessoaFisica == null)
          {
            return NotFound();
          }

          _context.pessoaFisicas.Remove(pessoaFisica);
          await _context.SaveChangesAsync();

          return NoContent();
        }

        
    }
}
