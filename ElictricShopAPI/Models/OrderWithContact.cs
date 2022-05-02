using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElictricShopAPI.Models
{    public class OrderWithContact
    {
        public string mail { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public List<ProductOrder> products { get; set; }
    }

    public class ProductOrder
    {
        public int id { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public double price { get; set; }
    }
}
