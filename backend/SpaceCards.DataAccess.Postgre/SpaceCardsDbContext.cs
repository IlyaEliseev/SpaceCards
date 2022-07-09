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

        public DbSet<Card> Cards { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GuessedCard> GuessedCards { get; set; }

        public DbSet<CardGuessingStatistics> CardsGuessingStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpaceCardsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
