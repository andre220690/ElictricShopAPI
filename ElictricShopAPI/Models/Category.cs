using System.Collections.Generic;

namespace ElictricShopAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategotyName { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}