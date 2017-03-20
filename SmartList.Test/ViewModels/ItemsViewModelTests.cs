using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PCLStorage;
using Plugin.Geolocator.Abstractions;

namespace SmartList.Test
{
    [TestFixture ()]
    public class ItemsViewModelTests
    {
        [Test ()]
        public void ItemsViewModel_LoadItems_CollectionChanged ()
        {
            // SETUP
            var items = new List<Item> ();
            items.Add (new Item () { CategoryId = "3", Id = Guid.NewGuid ().ToString (), Name = "Item-1" });
            items.Add (new Item () { CategoryId = "4", Id = Guid.NewGuid ().ToString (), Name = "Item-2" });
            items.Add (new Item () { CategoryId = "4", Id = Guid.NewGuid ().ToString (), Name = "Item-3" });

            var geoMock = new Mock<IGeolocatorService> ();
            var localNotificationMock = new Mock<ILocalNotification> ();
            var loadItemsCommandMock = new Mock<ICommandResult<List<Item>>> ();
            loadItemsCommandMock.Setup (lic => lic.Result).Returns (items);
            loadItemsCommandMock.Setup (lic => lic.Execute (It.IsAny<object> ())).Raises (i => i.Executed += null, this, new EventArgs ());
            var deleteCommandMock = new Mock<ICommandResult<Item>> ();

            var itemsViewModel = new ItemsViewModel (
                geoMock.Object,
                localNotificationMock.Object,
                loadItemsCommandMock.Object,
                deleteCommandMock.Object);

            bool isRefreshingCalled = false;
            itemsViewModel.PropertyChanged += (sender, e) => {
                isRefreshingCalled = true;

            };

            bool clearCalled = false;
            int addCalled = 0;
            itemsViewModel.Items.CollectionChanged += (sender, e2) => {
                if (e2.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                {
                    clearCalled = true;
                }

                if (e2.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    addCalled++;
                }
            };

            // ACTION
            itemsViewModel.LoadCommand.Execute (null);

            // ASSERT
            Assert.IsTrue (isRefreshingCalled);
            Assert.IsTrue (clearCalled);
            Assert.IsTrue (addCalled == 3);

        }
    }
}
