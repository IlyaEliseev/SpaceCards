using Microsoft.EntityFrameworkCore;
using SpaceCards.DataAccess.Postgre.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<GroupEntity>
    {
        public void Configure(EntityTypeBuilder<GroupEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(Domain.Model.Group.MAX_NAME_LENGTH)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired(true);

            builder.HasMany(x => x.Cards)
                .WithOne(x => x.Group)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}
