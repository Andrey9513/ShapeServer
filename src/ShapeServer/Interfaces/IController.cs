using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.Interfaces
{
    public interface IController<TDto, TEntity> : IReadOnlyController<TDto, TEntity>
        where TEntity : class, IBaseEntity
        where TDto : class, IBaseDto
    {

        Task<IActionResult> Put(int id, [FromBody]TDto dto);

        Task<ActionResult<TDto>> Post([FromBody]TDto dto);

        Task<ActionResult<TDto>> Delete(int id);
    }
}
