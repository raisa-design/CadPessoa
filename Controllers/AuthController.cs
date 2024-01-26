using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using CadPessoa.Api.Domain.Entidades;
using CadPessoa.Api.Extensions;
using CadPessoa.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CadPessoa.Api.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { live = true });
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar([FromBody] UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            //var usuario = new Usuario
            //{ 
            //    UserName = usuarioRegistro.
            
            
            //};



            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                return CustomResponse(await GerarJwt(usuarioRegistro.Email));
            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha,
                false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await GerarJwt(usuarioLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha incorretos");
            return CustomResponse();
        }

        private async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await ObterClaimsUsuario(claims, user);
            var encodedToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, user, claims);
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            return new UsuarioRespostaLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        //[HttpPut("upload-image/{pessoaId}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> UploadImage(int pessoaId)
        //{
        //    try
        //    {
        //        var pessoa = await _data.Pessoas.Where(p => p.Id == pessoaId).FirstOrDefaultAsync();

        //        var file = Request.Form.Files[0];
        //        Console.Write("|||||||||||||||||||||||||||||||||||||||||||||||");
        //        Console.WriteLine(file);

        //        if (file.Length > 0)
        //        {
        //            DeleteImagem(pessoa.Image);
        //            pessoa.Image = await SaveImage(file);
        //        }
        //        _data.Pessoas.Update(pessoa);
        //        _data.SaveChanges();
        //        //var ProdutoRetorno = await _produtoService.UpdateProduto(produtoId, produto);

        //        return Ok(pessoa);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return this.StatusCode(StatusCodes.Status500InternalServerError, $" {ex.Message}");
        //    }

        //}

        //[HttpPost("upload-image2")]
        //[AllowAnonymous]
        //public async Task<string> SaveImage(IFormFile imageFile)
        //{
        //    try
        //    {
        //        string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
        //                                         .Take(10)
        //                                         .ToArray()
        //                                         ).Replace(' ', '-');
        //        imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

        //        var imagePath = Path.Combine(_hostEnviroment.ContentRootPath, @"Resources/Images", imageName);
        //        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        //        {
        //            await imageFile.CopyToAsync(fileStream);
        //        }
        //        return imageName;
        //    }
        //    catch (Exception exeption)
        //    {
        //        return exeption.ToString();
        //    }
        //}
        //[NonAction]

        //public void DeleteImagem(string imageName)
        //{
        //    var imagePath = Path.Combine(_hostEnviroment.ContentRootPath, @"Resources/Images", imageName);
        //    if (System.IO.File.Exists(imagePath))
        //        System.IO.File.Delete(imagePath);
        //}


        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}