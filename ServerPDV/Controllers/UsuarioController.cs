using Microsoft.AspNetCore.Mvc;
using ServerPDV.Models;
using ServerPDV.Repository;

namespace ServerPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : CrudController<Usuario>
    {
        private readonly UsuarioRepository _usuarioRepository;
        public UsuarioController(UsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        [HttpGet("login")]
        public async Task<ActionResult<Usuario>> GetUsuarioByLoginESenha(string login)
        {
            var usuario = await _usuarioRepository.GetByLogin(login);

            if (usuario == null)
            {
                return NotFound(new { mensagem = "Usuário não encontrado no sistema" });
            }

            return Ok(usuario);
        }
    }
}
