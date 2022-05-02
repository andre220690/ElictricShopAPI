using ElictricShopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ElictricShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ApplicationDbContext db;
        JsonSerializerOptions options;

        public ProductsController(ApplicationDbContext db)
        {
            this.db = db;
            options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = false
            };
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategory(int categoryId)
        {
            var list = await db.Products.Where(u => u.SubCategoryId == categoryId).ToArrayAsync();
            var result = list.Select(p => JsonConvert.SerializeObject(JsonObjectCatalog(p))).ToArray();

            return result;
        }

        [HttpGet("allCategory")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllCategory()
        {
            var cat = await db.Categories.Include(u => u.SubCategories).ToListAsync();
            return cat.Select(s => JsonConvert.SerializeObject(new
            {
                category = s.CategotyName,
                subCategory = s.SubCategories.Select(u=> JsonConvert.SerializeObject(new 
                {
                    subName = u.SubCategoryName,
                    subId = u.Id
                })).ToArray()
            })).ToArray();
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<string>>> GetAll()// Первые 100 товаров из наличия
        {
            var list = await db.Products.Take(100).ToListAsync();

            var result = list.Select(p => JsonConvert.SerializeObject(JsonObjectCatalog(p))).ToArray();
            return result;
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<string>> GetId(int id)// По Id
        {
            var prod = await db.Products.FindAsync(id);
            return JsonConvert.SerializeObject(JsonObjectCatalog(prod));
        }

        [HttpGet("analog/{analogId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetAnalog(int analogId)// Аналоги
        {
            var list = await db.Products.Where(p => p.AnalogId == analogId).Take(100).ToListAsync();
            var result = list.Select(p => JsonConvert.SerializeObject(JsonObjectCatalog(p))).ToList();
            return result;
        }

        [HttpGet("complect/{complectId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetComplect(int complectId)//Загрузка комплектов
        {
            var obj = await db.Products.Include(u => u.Complects).Where(w => w.Id == complectId).FirstAsync();
            await db.Complects.Include(u => u.Products).ToListAsync();
            var list = obj.Complects.SelectMany(u => u.Products.ToList()).Distinct().ToList();
            var result = list.Select(p => JsonConvert.SerializeObject(JsonObjectCatalog(p))).ToList();
            return result;
        }

        [HttpGet("info/{id}")]
        public async Task<ActionResult<string>> GetInfo(int id)
        {
            await db.Products.Include(u => u.ParameterModel).ToListAsync();
            var model = db.Products.Find(id).ParameterModel.Model.ToString().Split("??").ToArray();
            var param = db.Products.Find(id).Parameter.Split("??").ToArray();
            string[] resultParam = new string[model.Length];
            for(int i = 0; i < model.Length; i++)
                resultParam[i] = model[i]+": "+param[i];
            var obj = db.Products.Find(id);
            return JsonConvert.SerializeObject(new
            {
                Name = obj.Name,
                Parameters = resultParam,
                Discription = obj.Discription,
                id = obj.Id,
                Image = obj.Image
            });
        }

        [HttpGet("search/{search}")]
        public async Task<ActionResult<IEnumerable<string>>> GetSearch(string search)
        {
            var listResult = new List<Product>();
            var listName = await db.Products.Where(u => u.Name.Contains(search)).ToListAsync();
            listResult.AddRange(listName);
            var listArticul = await db.Products.Where(u => u.Articul.Contains(search)).ToListAsync();
            listResult.AddRange(listArticul);
            var listManuf = await db.Products.Where(u => u.Manufacturer.Contains(search)).ToListAsync();
            listResult.AddRange(listManuf);
            var listDiscr = await db.Products.Where(u => u.Discription.Contains(search)).ToListAsync();
            listResult.AddRange(listDiscr);
            var result = listResult.Distinct().Select(u=> JsonConvert.SerializeObject(JsonObjectCatalog(u))).ToList();
            return result;
        }        

        [HttpPost("order")]
        public async Task PostOrder(OrderWithContact post)
        {
            Order order = new Order
            {
                Date = DateTime.Now,
                Email = post.mail,
                Name = post.name,
                Phone = post.phone
            };
            db.Orders.Add(order);
            foreach(var item in post.products)
            {
                order.CountToOrder.Add(new CountToOrder { ProductId = item.id, Count = item.count });
            }
            await db.SaveChangesAsync();
        }

        [HttpGet("getmanufacurer")]
        public async Task<ActionResult<IEnumerable<string>>> GetManufacturer()
        {
            var obj = await db.Products.GroupBy(u => u.Manufacturer).Select(u => u.Key).ToListAsync();
            obj.Remove(null);
            return obj;
        }


        public object JsonObjectCatalog(Product prod)
        {
            return new
            {
                Id = prod.Id,
                Name = prod.Name,
                Image = prod.Image,
                Articul = prod.Articul,
                AnalogId = prod.AnalogId,
                isComplect = prod.isComplect,
                Availability = prod.Availability,
                Price = prod.Price,
                Manufacture = prod.Manufacturer
            };
        }
    }
}
