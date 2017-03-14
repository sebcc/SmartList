using System;
using System.Collections.Generic;
using System.Linq;
using PCLStorage;

namespace SmartList
{
    public class AddItemViewModel : BaseViewModel
    {
        readonly IFolder rootFolder;

        string name;
        Item newItem;

        AddItemCommand addCommand;
        readonly ICategoriesService categoryService;

        public AddItemViewModel (IFolder rootFolder, ICategoriesService categoryService)
        {
            this.categoryService = categoryService;
            this.rootFolder = rootFolder;
            this.addCommand = new AddItemCommand (this.rootFolder);
            this.newItem = new Item ();
            this.newItem.Id = Guid.NewGuid ().ToString ();
        }

        public AddItemCommand AddCommand
        {
            get
            {
                return this.addCommand;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.newItem.Name = value;
                OnPropertyChanged ();
            }
        }

        public Item Item
        {
            get
            {
                return this.newItem;
            }
        }

        public void CategoryIndexSelected (int index)
        {
            var categories = this.categoryService.FetchCategories ();
            this.Item.CategoryId = categories.ElementAt (index).Id;
        }

        public List<Category> Categories
        {
            get
            {
                return this.categoryService.FetchCategories ();
            }
        }
    }
}
