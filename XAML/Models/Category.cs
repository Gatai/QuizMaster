using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAML.Models
{
    //Hämta ifrån ett annat api och får ner det i en array
    public class CategoryRoot
    {
        [JsonProperty("trivia_categories")]
        public Category[] Categories { get; set; }
    }

    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string CategoryName { get; set; }
    }


}
