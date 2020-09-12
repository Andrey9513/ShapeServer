using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShapeServer.Controllers.Models;
using ShapeServer.DataAccess;
using ShapeServer.DataAccess.Models;
using ShapeServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShapesEJRecordController : BaseControllerr<ShapeEJRecordDto, ShapeEJRecord>
    {
        public ShapesEJRecordController(ShapesDbContext dataContext, IMapper mapper, ILoggerFactory loggerFactory) : base(dataContext, mapper, loggerFactory)
        {
        }
    }
}
