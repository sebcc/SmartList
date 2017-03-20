using System;
using System.Collections.Generic;
using System.Windows.Input;
using Newtonsoft.Json;
using PCLStorage;

namespace SmartList
{
    public class LoadItemsCommand : ICommandResult<List<Item>>
    {
        IFolder rootFolder;
        List<Item> items = new List<Item> ();

        public LoadItemsCommand (IFolder rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public event EventHandler CanExecuteChanged;

        public event EventHandler Executed;

        public List<Item> Result
        {
            get
            {
                return this.items;
            }
        }

        public bool CanExecute (object parameter)
        {
            return true;
        }

        public void Execute (object parameter)
        {
            var itemsFolder = this.rootFolder.CreateFolderAsync ("items", CreationCollisionOption.OpenIfExists);
            itemsFolder.Wait ();
            var itemsFile = itemsFolder.Result.CreateFileAsync ("items.data", CreationCollisionOption.OpenIfExists);
            itemsFile.Wait ();
            var itemsContent = itemsFile.Result.ReadAllTextAsync ();
            itemsContent.Wait ();
            if (!string.IsNullOrWhiteSpace (itemsContent.Result))
            {
                this.items = JsonConvert.DeserializeObject<List<Item>> (itemsContent.Result);
            }


            if (this.Executed != null)
            {
                this.Executed.Invoke (this, new EventArgs ());
            }
        }
    }
}
