using ShapeServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.Controllers.Models
{
    public class ShapeDto : IBaseDto
    {
        public int Id { get; set; }

        public int Identifier { get; set; }

        public List<decimal> Parameters { get; set; }

        public int? ParentId { get; set; }
        
    }
}
