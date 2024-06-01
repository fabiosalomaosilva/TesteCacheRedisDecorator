using Microsoft.AspNetCore.Mvc;
using TesteCacheRedisDecorator.Infraestructure.Interfaces;
using TesteCacheRedisDecorator.Models;


namespace TesteCacheRedisDecorator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecosController(IRepository<Endereco> repository) : ControllerBase
    {
        // GET: api/<EnderecosController>
        [HttpGet]
        public async Task<IActionResult> Get(int take, int skip)
        {
            var result = await repository.GetAllAsync(10, 0);
            return Ok(result);
        }

        // GET api/<EnderecosController>/5
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

        // POST api/<EnderecosController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Endereco endereco)
        {
            if (endereco == null)
            {
                return BadRequest();
            }
            var result = await repository.AddAsync(endereco);
            return Ok(result);
        }

        // PUT api/<EnderecosController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Endereco endereco)
        {
            if (id != endereco.IdEndereco)
            {
                return BadRequest();
            }
            await repository.EditAsync(endereco);
            return NoContent();
        }

        // DELETE api/<EnderecosController>/5
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
