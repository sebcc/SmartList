using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PCLStorage;

namespace SmartList.Test
{
    [TestFixture ()]
    public class AddItemCommandTests
    {

        [Test ()]
        public async Task Execute_AddItemNoOtherItems ()
        {
            // SETUP
            var item = new Item () {
                CategoryId = "catid",
                Name = "Name1",
                Id = Guid.NewGuid ().ToString ()
            };
            var fileStub = new FileStub (string.Empty);

            var rootFolderMock = new Mock<IFolder> ();
            var itemsFolderMock = new Mock<IFolder> ();
            itemsFolderMock.Setup (ifm => ifm.GetFileAsync ("items.data", default (CancellationToken))).ReturnsAsync (fileStub);
            rootFolderMock.Setup (rf => rf.CreateFolderAsync ("items", CreationCollisionOption.OpenIfExists, default (CancellationToken))).ReturnsAsync (itemsFolderMock.Object);

            // ACTION
            var called = false;
            var command = new AddItemCommand (rootFolderMock.Object);
            command.Added += (sender, e) => {
                called = true;
            };
            command.Execute (item);


            // ASSERT
            Assert.IsTrue (called);
            Assert.AreEqual (JsonConvert.SerializeObject (new List<Item> () { item }), fileStub.WrittenContent);
        }
    }
}
