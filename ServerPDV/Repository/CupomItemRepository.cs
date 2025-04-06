using ServerPDV.Interfaces;
using ServerPDV.Models;
using Servidor_Frota.API.Repositories;

namespace ServerPDV.Repository
{
    public class CupomItemRepository : CrudRepository<CupomItem>, ICrud<CupomItem>
    {
        public CupomItemRepository(CaixaSupermercadoContext context) : base(context)
        {
        }
    }
}
