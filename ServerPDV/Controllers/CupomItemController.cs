using Microsoft.AspNetCore.Mvc;
using ServerPDV.Models;
using ServerPDV.Repository;


namespace ServerPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CupomItemController : CrudController<CupomItem>
    {
        private readonly CupomItemRepository _cupomRepository;
        public CupomItemController(CupomItemRepository cupomItemRepository) : base(cupomItemRepository)
        {
            _cupomRepository = cupomItemRepository;
        }
    }
}
