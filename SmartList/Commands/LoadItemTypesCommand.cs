using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using PCLStorage;

namespace SmartList
{
    public class LoadItemTypesCommand : ICommand
    {
        readonly IFolder rootFolder;

        List<string> placesTypes;


        public LoadItemTypesCommand (IFolder rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public event EventHandler CanExecuteChanged;

        public event EventHandler Loaded;

        public List<string> PlaceTypes
        {
            get
            {
                return this.placesTypes;
            }
            private set
            {
                this.placesTypes = value;
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
                var items = JsonConvert.DeserializeObject<List<Item>> (itemsContent);
                this.placesTypes = items.Select (i => i.Name).Distinct ().ToList ();
            }


            if (this.Loaded != null)
            {
                this.Loaded.Invoke (this, new EventArgs ());
            }
        }
    }
}
