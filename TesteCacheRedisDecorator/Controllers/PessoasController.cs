using Microsoft.AspNetCore.Mvc;
using TesteCacheRedisDecorator.Infraestructure.Interfaces;
using TesteCacheRedisDecorator.Models;


namespace TesteCacheRedisDecorator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController(IRepository<Pessoa> repository) : ControllerBase
    {
        // GET: api/<PessoasController>
        [HttpGet]
        public async Task<IActionResult> Get(int take, int skip)
        {
            var result = await repository.GetAllAsync(10, 0);
            return Ok(result);
        }

        // GET api/<PessoasController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await repository.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        // POST api/<PessoasController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pessoa pessoa)
        {
            if (pessoa == null)
            {
                return BadRequest();
            }
            var result = await repository.AddAsync(pessoa);
            return Ok(result);
        }

        // PUT api/<PessoasController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Pessoa pessoa)
        {
            if (id != pessoa.IdPessoa)
            {
                return BadRequest();
            }
            await repository.EditAsync(pessoa);
            return NoContent();
        }

        // DELETE api/<PessoasController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            await repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
