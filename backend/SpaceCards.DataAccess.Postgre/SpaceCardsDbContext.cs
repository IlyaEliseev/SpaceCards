using Microsoft.EntityFrameworkCore;
using SpaceCards.DataAccess.Postgre.Entites;

namespace SpaceCards.DataAccess.Postgre
{
    public class SpaceCardsDbContext : DbContext
    {
        public SpaceCardsDbContext(DbContextOptions<SpaceCardsDbContext> options)
            : base(options)
        {
        }

        public DbSet<CardEntity> Cards { get; set; }

        public DbSet<GroupEntity> Groups { get; set; }

        public DbSet<GuessedCardEntity> GuessedCards { get; set; }

        public DbSet<CardGuessingStatisticsEntity> CardsGuessingStatistics { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<SessionEntity> Sessions { get; set; }

        public DbSet<OAuthUserEntity> OAuthUserUsers { get; set; }

        public DbSet<OAuthUserTokenEntity> OAuthUserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpaceCardsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
