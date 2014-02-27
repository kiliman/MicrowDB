using Cirrious.MvvmCross.Community.Plugins.Sqlite.Wpf;
using MicrowDB.Client;
using MicrowDB.Tests.Models;
using NUnit.Framework;
using SyncTest.Console;

namespace MicrowDB.Tests
{
    [TestFixture]
    public class DocumentSessionTests
    {
        private IDocumentStore _store;

        [TestFixtureSetUp]
        public void Setup()
        {
            _store = new InMemoryDocumentStore(new MvxWpfSqLiteConnectionFactory()).Initialize();
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            _store.Dispose();
        }


        [Test]
        public void CanOpenSession()
        {
            using (var session = _store.OpenSession())
            {
                Assert.IsNotNull(session);
            }
        }

        [Test]
        public void CanSaveDocument()
        {
            var person = new Person
            {
                FirstName = "Michael",
                LastName = "Carter",
                Age = 43,
                Home = new Address
                {
                    City = "Chesapeake",
                    State = "VA"
                }
            };

            using (var session = _store.OpenSession())
            {
                session.Store(person);
                session.SaveChanges();
            }
        }

        [Test]
        public void CanLoadExistingDocument()
        {
            var person = new Person
            {
                FirstName = "Michael",
                LastName = "Carter",
                Age = 43,
                Home = new Address
                {
                    City = "Chesapeake",
                    State = "VA"
                }
            };

            using (var session = _store.OpenSession())
            {
                session.Store(person);
                session.SaveChanges();
            }

            using (var session = _store.OpenSession())
            {
                var existing = session.Load<Person>(person.Id);

                Assert.IsTrue(person.Id == existing.Id);
                Assert.IsTrue(person.FirstName == existing.FirstName);
                Assert.IsTrue(person.LastName == existing.LastName);
                Assert.IsTrue(person.Age == existing.Age);
            }
        }

        [Test]
        public void LoadingSameDocumentWillReturnSameObject()
        {
            var person = new Person
            {
                FirstName = "Michael",
                LastName = "Carter",
                Age = 43,
                Home = new Address
                {
                    City = "Chesapeake",
                    State = "VA"
                }
            };

            using (var session = _store.OpenSession())
            {
                session.Store(person);
                session.SaveChanges();
            }

            using (var session = _store.OpenSession())
            {
                var existing = session.Load<Person>(person.Id);
                var existing2 = session.Load<Person>(person.Id);

                Assert.IsTrue(existing == existing2);
            }
        }

        [Test]
        public void ModifyingDocumentAndCallingSaveChangesWillPersistChanges()
        {
            var person = new Person
            {
                FirstName = "Michael",
                LastName = "Carter",
                Age = 43,
                Home = new Address
                {
                    City = "Chesapeake",
                    State = "VA"
                }
            };

            using (var session = _store.OpenSession())
            {
                session.Store(person);
                session.SaveChanges();
            }

            using (var session = _store.OpenSession())
            {
                var existing = session.Load<Person>(person.Id);

                existing.FirstName = "Mike";

                session.SaveChanges();
            }

            using (var session = _store.OpenSession())
            {
                var existing = session.Load<Person>(person.Id);

                Assert.IsTrue(existing.FirstName == "Mike");
            }
        }

        [Test]
        public void ModifyingDocumentAndNotCallingSaveChangesWillNotPersistChanges()
        {
            var person = new Person
            {
                FirstName = "Michael",
                LastName = "Carter",
                Age = 43,
                Home = new Address
                {
                    City = "Chesapeake",
                    State = "VA"
                }
            };

            using (var session = _store.OpenSession())
            {
                session.Store(person);
                session.SaveChanges();
            }

            using (var session = _store.OpenSession())
            {
                var existing = session.Load<Person>(person.Id);

                existing.FirstName = "Mike";
            }

            using (var session = _store.OpenSession())
            {
                var existing = session.Load<Person>(person.Id);

                Assert.IsTrue(existing.FirstName == "Michael");
            }
        }


        [Test]
        public void CanDeleteExistingDocumentById()
        {
            var person = new Person
            {
                FirstName = "Michael",
                LastName = "Carter",
                Age = 43,
                Home = new Address
                {
                    City = "Chesapeake",
                    State = "VA"
                }
            };

            using (var session = _store.OpenSession())
            {
                session.Store(person);
                session.SaveChanges();
            }

            using (var session = _store.OpenSession())
            {
                var existing = session.Load<Person>(person.Id);
                Assert.IsNotNull(existing);
            }

            using (var session = _store.OpenSession())
            {
                session.Delete<Person>(person.Id);
                session.SaveChanges();
            }

            using (var session = _store.OpenSession())
            {
                var deleted = session.Load<Person>(person.Id);
                Assert.IsNull(deleted);
            }
        }

        [Test]
        public void CanDeleteExistingDocumentByObject()
        {
            var person = new Person
            {
                FirstName = "Michael",
                LastName = "Carter",
                Age = 43,
                Home = new Address
                {
                    City = "Chesapeake",
                    State = "VA"
                }
            };

            using (var session = _store.OpenSession())
            {
                session.Store(person);
                session.SaveChanges();
            }

            using (var session = _store.OpenSession())
            {
                var existing = session.Load<Person>(person.Id);

                session.Delete(existing);
                session.SaveChanges();
            }


            using (var session = _store.OpenSession())
            {
                var deleted = session.Load<Person>(person.Id);
                Assert.IsNull(deleted);
            }
        }


    }
}