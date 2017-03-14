using System;
using System.Collections.Generic;

namespace SmartList
{
    public interface ICategoriesService
    {
        List<Category> FetchCategories ();
    }
}
