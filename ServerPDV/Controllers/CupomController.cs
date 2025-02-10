using Microsoft.AspNetCore.Mvc;
using ServerPDV.Models;
using ServerPDV.Repository;

namespace ServerPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CupomController : CrudController<Cupom>
    {
        private readonly CupomRepository _cupomRepository;
        public CupomController(CupomRepository cupomRepository) : base(cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }
    }
}
