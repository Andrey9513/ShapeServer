using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShapeServer.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ShapeServer.DataAccess
{
    public class ShapesDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public ShapesDbContext(DbContextOptions<ShapesDbContext> options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<ShapesDbContext>();
            try
            {
                Database.Migrate();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "An error occurred during the migration");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (_loggerFactory != null)
            {
                optionsBuilder?
                .UseLoggerFactory(_loggerFactory)
                .EnableSensitiveDataLogging();
            }          
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges() => SaveChangesInternal().Result;

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => SaveChangesInternal();

        private async Task<int> SaveChangesInternal()
        {
            var changedEntities = ChangeTracker.Entries().Where(e => ( e.State == EntityState.Modified || e.State == EntityState.Added) && e.Entity is Shape).ToList();
            int saveResult;
            saveResult = await BaseSaveAsync();
            foreach (var changedEntity in changedEntities)
            {
                await CalculateFields((Shape)changedEntity.Entity, changedEntity.State);
            }
            saveResult = await BaseSaveAsync();
            return saveResult;
        }

        public async Task CalculateFields(Shape shape, EntityState state)
        {
            if (shape is null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            
            if (shape.ParentId is null)
            {
                shape.TreePath = $"{shape.Id}/";
            }
            else
            {
                var parent = await Set<Shape>().AsQueryable().SingleOrDefaultAsync(s => s.Id == shape.ParentId);
                if (parent is null)
                {
                    throw new InvalidOperationException($"Parent group tree with ID {shape.ParentId} does not exist");
                }
                shape.TreePath = $"{parent.TreePath}{shape.Id}";
                
            }
            shape.ShapeType = shape.Parameters.Count switch
            {
                1 => ShapeTypes.Circle,
                2 => shape.Parameters[0] == shape.Parameters[1] ? ShapeTypes.Square : ShapeTypes.Rectangle,
                _ => ShapeTypes.Unknown
            };
            var updatesRecord = new ShapeEJRecord()
            {
                ShapeId = shape.Id,
                UpdatedAt = DateTime.Now,
                CurrentArea = shape.ShapeType switch
                {
                    ShapeTypes.Circle => (decimal)Math.PI * shape.Parameters.Single() * shape.Parameters.Single(),
                    ShapeTypes.Rectangle => shape.Parameters[0] * shape.Parameters[1],
                    ShapeTypes.Square => shape.Parameters[0] * shape.Parameters[1],
                    _ => null
                }

            };
            Set<ShapeEJRecord>().Add(updatesRecord);

        }

        private async Task<int> BaseSaveAsync()
        {
            try
            {
                return await base.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var errorMessages = new List<string> { "A DbUpdateException was caught while saving changes" };
                errorMessages.AddRange(ex.Entries
                    .Select(x => $"Type {x.Entity.GetType().Name} was part of the problem"));

                var inners = GetInners(ex);
                errorMessages.AddRange(inners.Select(x => x.Message));

                var fullErrorMessage = string.Join("; ", errorMessages);
                throw new DbUpdateException(fullErrorMessage, ex);
            }
        }

        private IEnumerable<Exception> GetInners(Exception ex)
        {
            for (Exception e = ex; e != null; e = e.InnerException)
                yield return e;
        }


    }
}
