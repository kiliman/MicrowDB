using System.Security;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.Wpf;
using MicrowDB.Client;
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
    }
}
