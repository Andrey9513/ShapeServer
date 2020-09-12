using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShapeServer.DataAccess;
using ShapeServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.Controllers
{
    public class BaseControllerr<TDto, TEntity> : BaseReadOnlyController<TDto, TEntity>, IController<TDto, TEntity> 
        where TEntity : class, IBaseEntity
        where TDto: class, IBaseDto
    {
        public BaseControllerr(ShapesDbContext dataContext, IMapper mapper, ILoggerFactory loggerFactory) : base(dataContext, mapper, loggerFactory)
        {

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<TDto>> Delete(int id)
        {
            var result = await _dataContext.Set<TEntity>().AsQueryable().SingleOrDefaultAsync(e => e.Id == id);
            if (result is null)
            {
                return NotFound();
            }
            _dataContext.Set<TEntity>().Remove(result);
            await _dataContext.SaveChangesAsync();
            var mappedResult = _mapper.Map<TDto>(result);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<TDto>> Post([FromBody] TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _dataContext.Set<TEntity>().Add(entity);
            await _dataContext.SaveChangesAsync();

            var createdDto = _mapper.Map<TDto>(entity);
            return CreatedAtAction(nameof(this.GetData), new { id = entity.Id }, createdDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Put(int id, [FromBody] TDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var result = await _dataContext.Set<TEntity>().AsQueryable().SingleOrDefaultAsync(e => e.Id == id);

            if (result == null)
                return NotFound();

            _mapper.Map(dto, result);

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can not update entity");
                throw;
            }

            return NoContent();
        }
    }
}
