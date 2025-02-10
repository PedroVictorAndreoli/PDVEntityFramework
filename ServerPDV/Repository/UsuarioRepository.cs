using Microsoft.EntityFrameworkCore;
using ServerPDV.Interfaces;
using ServerPDV.Models;
using Servidor_Frota.API.Repositories;

namespace ServerPDV.Repository
{
    public class UsuarioRepository : CrudRepository<Usuario>, ICrud<Usuario>
    {
        public UsuarioRepository(CaixaSupermercadoContext context) : base(context)
        {
        }

        public async Task<Usuario> GetByLogin(string login)
        {
            return await _context.Usuario
                .FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}
