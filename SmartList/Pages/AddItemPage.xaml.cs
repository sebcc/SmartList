using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SmartList
{
    public partial class AddItemPage : ContentPage
    {
        public AddItemPage ()
        {
            InitializeComponent ();
        }

        void Handle_SelectedIndexChanged (object sender, System.EventArgs e)
        {
            var viewModel = this.BindingContext as AddItemViewModel;
            var picker = sender as Picker;

            viewModel.CategoryIndexSelected (picker.SelectedIndex);
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();

            var viewModel = this.BindingContext as AddItemViewModel;

            viewModel.AddCommand.Added += ItemAdded;

            foreach (var category in viewModel.Categories.Select (c => c.Name))
            {
                this.pckCategories.Items.Add (category);
            }
        }

        protected override void OnDisappearing ()
        {
            base.OnDisappearing ();
            var viewModel = this.BindingContext as AddItemViewModel;

            viewModel.AddCommand.Added -= ItemAdded;
        }

        private void ItemAdded (object sender, EventArgs ea)
        {
            Navigation.PopAsync (true);
        }
    }
}
