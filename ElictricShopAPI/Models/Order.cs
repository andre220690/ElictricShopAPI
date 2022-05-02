using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElictricShopAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<Product> Products { get; set; } = new();
        public List<CountToOrder> CountToOrder { get; set; } = new();

        //public static implicit operator Order(Order v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
