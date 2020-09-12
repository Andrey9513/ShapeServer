using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShapeServer.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.DataAccess.Maps
{
    public class ShapeMap : BaseEntityMap<Shape>
    {
        public override void Configure(EntityTypeBuilder<Shape> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.Configure(builder);

            builder.HasOne(e => e.Parent)
               .WithMany(e => e.Shapes)
               .HasForeignKey(e => e.ParentId)
               .HasConstraintName("FK_Shape_Shape");

            builder
                .HasIndex(u => u.Identifier)
                .IsUnique();

            builder
                .HasIndex(u => u.TreePath)
                .IsUnique();
        }
    }
}
