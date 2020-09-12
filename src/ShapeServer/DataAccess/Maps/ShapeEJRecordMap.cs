using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShapeServer.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.DataAccess.Maps
{
    public class ShapeEJRecordMap : BaseEntityMap<ShapeEJRecord>
    {

        public override void Configure(EntityTypeBuilder<ShapeEJRecord> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.Configure(builder);

            builder.HasOne(e => e.Shape)
                   .WithMany(e => e.ShapeEJRecords)
                   .HasForeignKey(e => e.ShapeId)
                   .HasConstraintName("FK_ShapeEJRecord_Shape");
        }
    }
}
