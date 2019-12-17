using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAML.Models
{
    //Hämta ifrån ett annat api och får ner det i en array
    class CategoryRoot
    {
        [JsonProperty("trivia_categories")]
        public Category[] QuizCategories { get; set; }
    }

    class Category
    {

    }
}
