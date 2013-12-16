using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Abo.Server.Domain.Entities;
using Abo.Server.Domain.Repositories;
using Abo.Server.Domain.Seedwork;
using Abo.Server.Domain.Seedwork.Specifications;

namespace Abo.Server.Application.Tests.Stubs
{
    internal class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _players = new List<User>();

        public User FirstMatching(Specification<User> spec)
        {
            return _players.FirstOrDefault(spec.SatisfiedBy().Compile());
        }

        public bool AnyMatching(Specification<User> spec)
        {
            return _players.Any(spec.SatisfiedBy().Compile());
        }

        public void Add(User user)
        {
            _players.Add(user);
        }

        User[] IUserRepository.AllMatching(Specification<User> spec, int limit)
        {
            throw new NotImplementedException();
        }

        public User[] TakeAllMatching<TOrderBy>(int count, Specification<User> spec, Expression<Func<User, TOrderBy>> orderBy)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> AllMatching(Specification<User> spec, int limit = 0)
        {
            return _players.Where(spec.SatisfiedBy().Compile());
        }

        public void Attach<T>(T entity) where T : Entity
        {
        }
    }
}
