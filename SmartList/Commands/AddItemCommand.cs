using System;
using System.Collections.Generic;
using System.Windows.Input;
using Newtonsoft.Json;
using PCLStorage;

namespace SmartList
{
    public class AddItemCommand : ICommand
    {
        IFolder rootFolder;

        List<Item> items;


        public AddItemCommand (IFolder rootFolder)
        {
            this.rootFolder = rootFolder;
            this.items = new List<Item> ();
        }

        public event EventHandler CanExecuteChanged;

        public event EventHandler Added;

        public bool CanExecute (object parameter)
        {
            return true;
        }

        public async void Execute (object parameter)
        {
            var item = parameter as Item;
            var itemsFolder = await this.rootFolder.CreateFolderAsync ("items", CreationCollisionOption.OpenIfExists);

            var itemsFile = await itemsFolder.GetFileAsync ("items.data");
            var itemsContent = await itemsFile.ReadAllTextAsync ();
            if (!string.IsNullOrWhiteSpace (itemsContent))
            {
                this.items = JsonConvert.DeserializeObject<List<Item>> (itemsContent);
            }

            this.items.Add (item);

            itemsContent = JsonConvert.SerializeObject (this.items);
            await itemsFile.WriteAllTextAsync (itemsContent);


            if (this.Added != null)
            {
                this.Added.Invoke (this, new EventArgs ());
            }
        }
    }
}
