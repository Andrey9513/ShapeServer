using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.DataAccess.Models
{
    public class ShapeEJRecord : BaseEntity
    {
        public int ShapeId { get; set; }

        public DateTime UpdatedAt { get; set; }

        public decimal? CurrentArea { get; set; }

        public virtual Shape Shape { get; set; } = null!;
    }
}
