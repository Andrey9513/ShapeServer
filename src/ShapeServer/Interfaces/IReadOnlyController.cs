using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.Interfaces
{
    public interface IReadOnlyController <TDto,TEntity>
        where TEntity : class, IBaseEntity
        where TDto : class, IBaseDto
    {
        Task<ActionResult<List<TDto>>> GetData();

        Task<ActionResult<TDto>> GetData(int id);
    }
}
