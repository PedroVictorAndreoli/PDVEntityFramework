using ServerPDV.Interfaces;
using ServerPDV.Models;
using Servidor_Frota.API.Repositories;

namespace ServerPDV.Repository
{
    public class CupomRepository : CrudRepository<Cupom>, ICrud<Cupom>
    {
        public CupomRepository(CaixaSupermercadoContext context) : base(context)
        {

        }
    }
}
