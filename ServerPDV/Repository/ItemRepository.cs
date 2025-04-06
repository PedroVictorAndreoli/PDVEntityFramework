using Microsoft.EntityFrameworkCore;
using ServerPDV.Interfaces;
using ServerPDV.Models;
using Servidor_Frota.API.Repositories;

namespace ServerPDV.Repository
{
    public class ItemRepository : CrudRepository<Item>, ICrud<Item>
    {
        public ItemRepository(CaixaSupermercadoContext context) : base(context)
        {
        }
        
    }
}
