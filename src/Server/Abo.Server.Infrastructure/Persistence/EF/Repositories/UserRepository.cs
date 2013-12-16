using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Abo.Server.Domain.Entities;
using Abo.Server.Domain.Repositories;
using Abo.Server.Domain.Seedwork.Specifications;

namespace Abo.Server.Infrastructure.Persistence.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnitOfWork _uow;

        public UserRepository(UnitOfWork uow)
        {
            _uow = uow;
        }

        public User FirstMatching(Specification<User> spec)
        {
            var player = _uow.Players.FirstOrDefault(spec.SatisfiedBy());
            return player;
        }

        public bool AnyMatching(Specification<User> spec)
        {
            return _uow.Players.Any(spec.SatisfiedBy());
        }

        public void Add(User user)
        {
            _uow.Players.Add(user);
        }

        public User[] AllMatching(Specification<User> spec, int limit = 0)
        {
            var players = _uow.Players.Where(spec.SatisfiedBy());
            if (limit > 0)
                players = players.Take(limit);
            return players.ToArray();
        }

        public User[] TakeAllMatching<TOrderBy>(int count, Specification<User> spec, Expression<Func<User, TOrderBy>> orderBy)
        {
            if (spec != null)
                return _uow.Players.Where(spec.SatisfiedBy()).OrderByDescending(orderBy).Take(count).ToArray();
            return _uow.Players.OrderByDescending(orderBy).Take(count).ToArray();
        }
    }
}
