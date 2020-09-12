using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.DataAccess.Models
{
    public class Shape : BaseEntity
    {
        public ShapeTypes ShapeType { get ; set;}

        public int Identifier { get; set; }

        public List<decimal> Parameters { get; set; }

        public int? ParentId { get; set; }

        public string TreePath { get; set; }

        public virtual Shape Parent { get; set; } = null!;

        public virtual ICollection<Shape> Shapes { get; } = new List<Shape>();

        public virtual ICollection<ShapeEJRecord> ShapeEJRecords { get; } = new List<ShapeEJRecord>();

    }

    public enum ShapeTypes { Circle, Square, Rectangle, Unknown }
}
