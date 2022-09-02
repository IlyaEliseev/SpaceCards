using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.Email)
                .HasMaxLength(Domain.User.MAX_EMAIL_LENGTH)
                .IsRequired(true);

            builder.Property(x => x.PasswordHash).IsRequired(true);

            builder.Property(x => x.RegistrationData).IsRequired(true);
        }
    }
}
