using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace SmartList
{
    public class CategoriesService : ICategoriesService
    {
        private List<Category> categories = new List<Category> ();

        public CategoriesService ()
        {
            var assembly = typeof (CategoriesService).GetTypeInfo ().Assembly;
            Stream stream = assembly.GetManifestResourceStream ("SmartList.Resources.Categories.txt");
            string text = "";

            using (var reader = new System.IO.StreamReader (stream))
            {
                text = reader.ReadToEnd ();
            }

            var types = JsonConvert.DeserializeObject<dynamic> (text);

            foreach (var type in types.data)
            {
                categories.Add (new Category () { Name = type.Name, Id = type.Id, GooglePlaceName = type.GooglePlaceName });
            }
        }

        public List<Category> FetchCategories ()
        {
            return this.categories;
        }
    }
}
