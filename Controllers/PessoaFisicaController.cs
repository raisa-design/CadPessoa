using CadPessoa.Api.Data;
using CadPessoa.Api.Domain.Entidades;
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
            if (_context.PessoaFisicas== null)
            {
                return NotFound();
            }
            return await _context.PessoaFisicas.ToListAsync();
        }

        // GET: api/Anuncios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaFisica>> GetAnuncio([FromBody] Guid id)
        {
            if (_context.PessoaFisicas == null)
            {
                return NotFound();
            }
            var anuncio = await _context.PessoaFisicas
                .FirstOrDefaultAsync();

            if (anuncio == null)
            {
                return NotFound();
            }

            return anuncio;
        }

        // PUT: api/Anuncios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnuncio([FromBody] PessoaFisica model)
        {

            var pessoaFisica = await _context.PessoaFisicas
                .FirstOrDefaultAsync();

            pessoaFisica = new PessoaFisica
            {
                Id = model.Id, 
                Nome = model.Nome,
                SobreNome = model.SobreNome,
                Cpf = model.Cpf,
                DataNascimento = model.DataNascimento,
                Email = model.Email,
                Rg = model.Rg

            };

            _context.PessoaFisicas.Update(pessoaFisica);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnuncioExists(pessoaFisica.Id))
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
        [HttpPost]
        public async Task<ActionResult<PessoaFisica>> PostAnuncio([FromBody] PessoaFisica pessoaFisica)
        {
            if (_context.PessoaFisicas == null)
            {
                return Problem("Entity set 'DataContext.Anuncios'  is null.");
            }
            _context.PessoaFisicas.Add(pessoaFisica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnuncio", new { id = pessoaFisica.Id }, pessoaFisica);
        }

        // DELETE: api/Anuncios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnuncio(Guid id)
        {
            if (_context.PessoaFisicas == null)
            {
                return NotFound();
            }
            var anuncio = await _context.PessoaFisicas.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (anuncio == null)
            {
                return NotFound();
            }

            _context.PessoaFisicas.Remove(anuncio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnuncioExists(Guid id)
        {
            return (_context.PessoaFisicas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
