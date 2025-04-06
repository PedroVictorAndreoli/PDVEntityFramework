using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using ServerPDV.Repository;
using Servidor_Frota.API.Repositories;

namespace ServerPDV.Controllers
{
    [ApiController]
    public abstract class CrudController<T> : Controller where T : class
    {
        protected readonly CrudRepository<T> _crudRepository;

        public CrudController(CrudRepository<T> crudRepository)
        {
            _crudRepository = crudRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            return Ok(await _crudRepository.SelecionarTodos());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<T>> DeleteObject(int id)
        {
            T entity = await _crudRepository.Selecionar(id);
            _crudRepository.Delete(entity);
            return Ok(entity);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> Get(int id)
        {
            return Ok(await _crudRepository.Selecionar(id));
        }

        [HttpPut]
        public async Task<ActionResult<T>> Update([FromBody] T entity)
        {
            return Ok(await _crudRepository.Update(entity));
        }

        [HttpPost]
        public async Task<ActionResult<T>> Salvar([FromBody] T entity)
        {
            return Ok(await _crudRepository.Salvar(entity));
        }


        [HttpGet("executar-query")]
        public async Task<ActionResult<IEnumerable<T>>> ExecutarQueryCustomizada(
             [FromHeader(Name = "campos")] List<string> campos,
             [FromHeader(Name = "parametros")] string parametros)
        {
            if (campos == null || !campos.Any())
                return BadRequest("Os campos a serem retornados devem ser especificados.");


            var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
                return BadRequest("A entidade não possui o atributo [Table].");

            string nomeTabela = tableAttribute.Name;


            string camposString = string.Join(", ", campos);
            string query = $"SELECT {camposString} FROM {nomeTabela}";


            query += $" WHERE {parametros}";

            Console.WriteLine($"Query Executed: {query}");
            var resultado = await _crudRepository.ExecutarQuery(query);
            return Ok(resultado);
        }
    }
}
