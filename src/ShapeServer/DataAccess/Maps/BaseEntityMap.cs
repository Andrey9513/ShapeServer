using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShapeServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeServer.DataAccess.Maps
{
    public class BaseEntityMap<TEntity> : IEntityTypeConfiguration<TEntity>
         where TEntity : class, IBaseEntity
    {
        protected virtual string? TableName => null;

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable(TableName ?? typeof(TEntity).Name)
                .HasKey(x => x.Id);
        }
    }
}
