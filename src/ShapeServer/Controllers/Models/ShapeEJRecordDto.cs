using ShapeServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.Controllers.Models
{
    public class ShapeEJRecordDto : IBaseDto
    {
        public int Id { get; set; }

        public int ShapeId { get; set; }

        public DateTime UpdatedAt { get; set; }

        public decimal? CurrentArea { get; set; }
    }
}
