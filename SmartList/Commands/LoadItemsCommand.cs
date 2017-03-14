using System;
using System.Collections.Generic;
using System.Windows.Input;
using Newtonsoft.Json;
using PCLStorage;

namespace SmartList
{
    public class LoadItemsCommand : ICommand
    {
        IFolder rootFolder;
        List<Item> items = new List<Item> ();

        public LoadItemsCommand (IFolder rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public event EventHandler CanExecuteChanged;

        public event EventHandler Loaded;

        public List<Item> Items
        {
            get
            {
                return this.items;
            }
            private set
            {
                this.items = value;
            }
        }

        public bool CanExecute (object parameter)
        {
            return true;
        }

        public async void Execute (object parameter)
        {
            var itemsFolder = await this.rootFolder.CreateFolderAsync ("items", CreationCollisionOption.OpenIfExists);

            var itemsFile = await itemsFolder.CreateFileAsync ("items.data", CreationCollisionOption.OpenIfExists);
            var itemsContent = await itemsFile.ReadAllTextAsync ();
            if (!string.IsNullOrWhiteSpace (itemsContent))
            {
                this.items = JsonConvert.DeserializeObject<List<Item>> (itemsContent);
            }


            if (this.Loaded != null)
            {
                this.Loaded.Invoke (this, new EventArgs ());
            }
        }
    }
}
