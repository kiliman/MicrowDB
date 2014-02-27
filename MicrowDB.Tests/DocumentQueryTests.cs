using System.Collections.Generic;
using System.Linq;
using System.Security;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.Wpf;
using MicrowDB.Client;
using MicrowDB.Tests.Models;
using NUnit.Framework;
using SyncTest.Console;

namespace MicrowDB.Tests
{
    [TestFixture]
    public class DocumentQueryTests
    {
        private IDocumentStore _store;
        private List<Person> _people;

        [TestFixtureSetUp]
        public void Setup()
        {
            _store = new InMemoryDocumentStore(new MvxWpfSqLiteConnectionFactory()).Initialize();

            _store.RegisterIndex(new PersonIndexMap());

            _people = new List<Person>()
            {
                new Person
                {
                    FirstName = "Michael",
                    LastName = "Carter",
                    Age = 43,
                    Home = new Address
                    {
                        City = "Chesapeake",
                        State = "VA"
                    }
                },
                new Person
                {
                    FirstName = "Lisa",
                    LastName = "Carter",
                    Age = 51,
                    Home = new Address
                    {
                        City = "Chesapeake",
                        State = "VA"
                    }
                },
                new Person
                {
                    FirstName = "Matthew",
                    LastName = "Carter",
                    Age = 16,
                    Home = new Address
                    {
                        City = "Chesapeake",
                        State = "VA"
                    }
                },
                new Person
                {
                    FirstName = "Sarah",
                    LastName = "Carter",
                    Age = 19,
                    Home = new Address
                    {
                        City = "Richmond",
                        State = "VA"
                    }
                }
            };

            using (var session = _store.OpenSession())
            {
                foreach (var person in _people)
                {
                    session.Store(person);
                }
                session.SaveChanges();
            }
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            _store.Dispose();
        }

        [Test]
        public void CanQueryIndex()
        {
            using (var session = _store.OpenSession())
            {
                var results = session.Query<PersonIndex>().ToList();

                Assert.IsTrue(results.Count == 4);
            }
        }

        [Test]
        public void CanQueryIndexWithCriteria()
        {
            using (var session = _store.OpenSession())
            {
                var results = session.Query<PersonIndex>()
                    .Where(x => x.Age < 21)
                    .ToList();

                Assert.IsTrue(results.Count == 2);
            }
        }

        [Test]
        public void CanQueryIndexWithOrderBy()
        {
            using (var session = _store.OpenSession())
            {
                var results = session.Query<PersonIndex>()
                    .OrderBy(x => x.Age)
                    .ToList();

                Assert.IsTrue(results[0].Age == 16);
            }
        }

        [Test]
        public void CanQueryIndexCaseInsensitive()
        {
            using (var session = _store.OpenSession())
            {
                var results = session.Query<PersonIndex>()
                    .Where(x => x.Home_City == "chesapeake")
                    .ToList();

                Assert.IsTrue(results.Count == 3);
            }
        }

        [Test]
        public void ModifyingDocumentAffectsIndex()
        {
            using (var session = _store.OpenSession())
            {
                var person = session.Load<Person>(_people[3].Id);
                person.Home.City = "Chesapeake";
                session.SaveChanges();

                var results = session.Query<PersonIndex>()
                    .Where(x => x.Home_City == "chesapeake")
                    .ToList();

                Assert.IsTrue(results.Count == 4);
            }
        }
    }
}
