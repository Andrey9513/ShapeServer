
using ShapeServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.Controllers.Models
{
    public class BaseDto : IBaseDto
    {
        public int Id { get; set; }
    }
}
