using System.Data.Entity;
using System.Linq;
using Abo.Server.Domain.Entities;
using Abo.Server.Infrastructure.Persistence.EF;
using NUnit.Framework;

namespace Abo.Server.Infrastructure.Tests.EntityFramework
{
    [TestFixture]
    public class EfTrackingTests //just understanding how Attach(track) works...
    {
        [TestFixtureSetUp]
        public void CreateDatabase()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<UnitOfWork>());
        }

        [Test]
        public void AttachShouldIgnoreOldValues()
        {
            var player = new User("Egor", "qwerty", "HUID", true, 24, "http://pushuri", "Belarus");

            using (var dc1 = new UnitOfWork())
            {
                dc1.UsersRepository.Add(player);
                player.Friends.Add(new User("Friend1", "2", "HUID2", true, 25, "http://pushuri2", "Norway"));
                dc1.SaveChanges();
            }

            //these changes should not be saved into db 
            player.ChangeName("ThisNameShouldNotBeSavedToDbOnNextSaveChanges");

            using (var dc2 = new UnitOfWork())
            {
                dc2.Attach(player);
                player.Friends.Add(new User("Friend2", "3", "HUID3", true, 26, "http://pushuri3", "Norway"));
                player.ChangeCountry("USA");
                dc2.SaveChanges();
            }

            using (var dc3 = new UnitOfWork())
            {
                User persistedUser = dc3.UsersRepository.FirstMatching(UserSpecification.Id(player.Id));
                Assert.AreEqual("Egor", persistedUser.Name);
                Assert.AreEqual("USA", persistedUser.Country);
                Assert.AreEqual(2, persistedUser.Friends.Count);

                persistedUser.Friends.Add(new User("Friend3", "4", "HUID4", true, 27, "http://pushuri4", "Norway"));
                dc3.SaveChanges();
            }

            //using (var dc4 = new UnitOfWork())
            //{
            //    dc4.Attach(player);
            //    Assert.AreEqual(3, player.Friends.Count);
            //}
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
        }
    }
}
