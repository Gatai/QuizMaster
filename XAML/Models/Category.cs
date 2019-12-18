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
        [JsonProperty("response_code")]
        public int ResponseCode { get; set; }

        [JsonProperty("results")]
        public Category[] Categories { get; set; }
    }

    public class Category
    {
        [JsonProperty("category")]
        public string CategoryName { get; set; }
    }


}
