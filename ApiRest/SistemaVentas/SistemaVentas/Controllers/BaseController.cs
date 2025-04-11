using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Services.Interfaces;

namespace SistemaVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class
    {
        protected readonly IService<T> _service;
        public BaseController(IService<T> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T entity)
        {
            var createdEntity = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = GetIdValue(createdEntity) }, createdEntity);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] T entity)
        {
            if (id != GetIdValue(entity))
            {
                return BadRequest();
            }

            try
            {
                await _service.UpdateAsync(id, entity);
            }
            catch
            {
                if (await _service.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }

        private int GetIdValue(T entity)
        {
            var propertyInfo = typeof(T).GetProperty("Id");
            if (propertyInfo != null)
            {
                return (int)propertyInfo.GetValue(entity);
            }
            throw new Exception("No Id property found");
        }
    }
}
