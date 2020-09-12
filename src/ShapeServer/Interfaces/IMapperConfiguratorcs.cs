using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.Interfaces
{
    public interface IMapperConfigurator
    {
        void Configure(IMapperConfigurationExpression config);
    }
}
