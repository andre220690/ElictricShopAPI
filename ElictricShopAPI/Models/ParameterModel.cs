using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElictricShopAPI.Models
{
    public class ParameterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public List<Product> Products { get; set; }

    }
}
