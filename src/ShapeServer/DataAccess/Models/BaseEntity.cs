using ShapeServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.DataAccess.Models
{
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}
