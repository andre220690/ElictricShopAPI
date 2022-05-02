using System.Collections.Generic;

namespace ElictricShopAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Availability { get; set; } //Наличие
        public string Articul { get; set; }
        public string Manufacturer { get; set; }
        public int? AnalogId { get; set; }
        public Analog Analog { get; set; }
        public bool isComplect { get; set; }
        public List<Complect> Complects { get; set; }
        public int Balance { get; set; }
        public string Image { get; set; }
        public string Discription { get; set; }
        public int? ParameterModelId { get; set; }
        public ParameterModel ParameterModel { get; set; }
        public string Parameter { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<Order> Orders { get; set; } = new();//Заказы
        public List<CountToOrder> CountToOrder { get; set; } = new();
    }
}
