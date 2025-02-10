using Microsoft.AspNetCore.Mvc;
using ServerPDV.Models;
using ServerPDV.Repository;
using Servidor_Frota.API.Repositories;

namespace ServerPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : CrudController<Item>
    {
        private readonly ItemRepository _itemRepository;
        public ItemController(ItemRepository itemRepository) : base(itemRepository)
        {
            _itemRepository = itemRepository;
        }
    }
}
