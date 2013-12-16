using Abo.Server.Domain.Seedwork.Specifications;

namespace Abo.Server.Domain.Entities
{
    public static class PublicMessageSpecification
    {
        public static Specification<PublicMessage> Author(User user)
        {
            return new DirectSpecification<PublicMessage>(m => m.Author == user);
        }
    }
}