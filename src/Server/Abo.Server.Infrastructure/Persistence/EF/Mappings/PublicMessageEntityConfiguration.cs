using System.Data.Entity.ModelConfiguration;
using Abo.Server.Domain.Entities;

namespace Abo.Server.Infrastructure.Persistence.EF.Mappings
{
    public class PublicMessageEntityConfiguration : EntityTypeConfiguration<PublicMessage>
    {
        public PublicMessageEntityConfiguration()
        {
        }
    }
}