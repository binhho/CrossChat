using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Abo.Server.Domain.Entities;
using Abo.Server.Domain.Seedwork.Specifications;

namespace Abo.Server.Domain.Repositories
{
    public interface IUserRepository : IRepository
    {
        /// <summary>
        /// Returns first User that matches specified spec
        /// </summary>
        User FirstMatching(Specification<User> spec);

        /// <summary>
        /// Returns players who match specified spec
        /// </summary>
        bool AnyMatching(Specification<User> spec);

        /// <summary>
        /// Adds User to repo
        /// </summary>
        void Add(User user);

        /// <summary>
        /// </summary>
        User[] AllMatching(Specification<User> spec, int limit = 0);

        /// <summary>
        /// </summary>
        User[] TakeAllMatching<TOrderBy>(int count, Specification<User> spec, Expression<Func<User, TOrderBy>> orderBy);
    }
}
