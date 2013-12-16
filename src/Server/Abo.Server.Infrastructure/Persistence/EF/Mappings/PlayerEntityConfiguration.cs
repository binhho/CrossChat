using System.Data.Entity.ModelConfiguration;
using Abo.Server.Domain.Entities;

namespace Abo.Server.Infrastructure.Persistence.EF.Mappings
{
    public class PlayerEntityConfiguration : EntityTypeConfiguration<User>
    {
        public PlayerEntityConfiguration()
        {
            HasMany(m => m.Friends)
                .WithMany()
                .Map(m =>
                     {
                         m.MapLeftKey("TargetId");
                         m.MapRightKey("TargetUserId");
                         m.ToTable("PlayerFriends");
                     });

            HasMany(m => m.PersonalBlackList)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("TargetId");
                    m.MapRightKey("TargetUserId");
                    m.ToTable("PlayerBlacklist");
                });
        }
    }
}
