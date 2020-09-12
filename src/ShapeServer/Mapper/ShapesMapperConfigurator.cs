using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShapeServer.Controllers.Models;
using ShapeServer.DataAccess.Models;
using ShapeServer.Interfaces;

namespace ShapeServer.Mapper
{
    public class ShapesMapperConfigurator : IMapperConfigurator
    {
        public void Configure(IMapperConfigurationExpression config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.CreateMap<Shape, ShapeDto>()
                .ReverseMap();
            config.CreateMap<ShapeDto, Shape>()
                .ReverseMap();

            config.CreateMap<ShapeEJRecord, ShapeEJRecordDto>()
                .ReverseMap();
            config.CreateMap<ShapeDto, Shape>()
                .ReverseMap();
        }
    }
}
