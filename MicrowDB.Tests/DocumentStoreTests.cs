using System.Security;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.Wpf;
using MicrowDB.Client;
using MicrowDB.Tests.Models;
using NUnit.Framework;

namespace MicrowDB.Tests
{
    [TestFixture]
    public class DocumentStoreTests
    {
        [Test]
        public void CanInitializeDocumentStore()
        {
            var store = new InMemoryDocumentStore(new MvxWpfSqLiteConnectionFactory());
            store.Initialize();
            Assert.IsNotNull(store.Connection);
        }

        [Test]
        public void CanRegisterIndex()
        {
            var store = new InMemoryDocumentStore(new MvxWpfSqLiteConnectionFactory()).Initialize();

            store.RegisterIndex(new PersonIndexMap());

            var map = store.GetIndexMapByEntityType(typeof (Person));
            Assert.IsNotNull(map);
        }

    }
}
