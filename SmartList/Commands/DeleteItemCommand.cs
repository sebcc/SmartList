using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using PCLStorage;

namespace SmartList
{
    public class DeleteItemCommand : ICommandResult<Item>
    {
        IFolder rootFolder;
        Item deletedItem;

        public DeleteItemCommand (IFolder rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public event EventHandler CanExecuteChanged;

        public event EventHandler Executed;

        public Item Result
        {
            get
            {
                return this.deletedItem;
            }
        }

        public bool CanExecute (object parameter)
        {
            return true;
        }

        public async void Execute (object parameter)
        {
            var itemId = parameter as string;
            var items = new List<Item> ();
            var itemsFolder = await this.rootFolder.CreateFolderAsync ("items", CreationCollisionOption.OpenIfExists);

            var itemsFile = await itemsFolder.GetFileAsync ("items.data");
            var itemsContent = await itemsFile.ReadAllTextAsync ();
            if (!string.IsNullOrWhiteSpace (itemsContent))
            {
                items = JsonConvert.DeserializeObject<List<Item>> (itemsContent);
            }

            var item = items.FirstOrDefault (i => i.Id == itemId);
            items.Remove (item);

            this.deletedItem = item;
            itemsContent = JsonConvert.SerializeObject (items);
            await itemsFile.WriteAllTextAsync (itemsContent);

            if (this.Executed != null)
            {
                this.Executed.Invoke (this, new EventArgs ());
            }
        }
    }
}
