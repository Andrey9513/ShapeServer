using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShapeServer.DataAccess;
using ShapeServer.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.Controllers
{
    public class BaseReadOnlyController<TDto, TEntity> : ControllerBase, IReadOnlyController<TDto, TEntity> 
        where TEntity: class, IBaseEntity
        where TDto : class, IBaseDto
    {
        protected  ShapesDbContext _dataContext { get; }
        protected  IMapper _mapper { get; }

        protected ILoggerFactory _loggerFactory;

        protected ILogger _logger;

        public BaseReadOnlyController(ShapesDbContext dataContext, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger(this.GetType().Name);
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<List<TDto>>> GetData()
        {
            var data = await  _dataContext.Set<TEntity>().AsQueryable().ToListAsync<TEntity>();
            if (data.Any())
            {
                var mappedData = _mapper.Map<TDto>(data);
                return Ok(mappedData);
            }
            else
            {
                var emptyList = new List<TDto>();
                return Ok(emptyList);
            }
            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<TDto>> GetData(int id)
        {
            var result = await _dataContext.Set<TEntity>().AsQueryable().SingleOrDefaultAsync(e => e.Id == id);
            if( result is null)
            {
                return NotFound();
            }
            
            var mappedResult = _mapper.Map<TDto>(result);
            return Ok(mappedResult);           
        }
    }
}
