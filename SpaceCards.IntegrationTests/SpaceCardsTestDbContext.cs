using Microsoft.EntityFrameworkCore;
using SpaceCards.DataAccess.Postgre;
using System;

namespace SpaceCards.IntegrationTests
{
    public class SpaceCardsTestDbContext : IDisposable
    {
        public readonly SpaceCardsDbContext _dbContext;
        public string ConnString;

        private bool _disposed;

        public SpaceCardsTestDbContext(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<SpaceCardsDbContext>();

            builder.UseNpgsql(connectionString);
            _dbContext = new SpaceCardsDbContext(builder.Options);

            _dbContext.Database.Migrate();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Database.EnsureDeleted();
                }

                _disposed = true;
            }
        }
    }
}
