using Microsoft.EntityFrameworkCore;

namespace ElictricShopAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Analog> Analogs { get; set; }
        public DbSet<Complect> Complects { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ParameterModel> ParameterModels { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {
            ////Database.EnsureDeleted();
            //Database.EnsureCreated();

            //Category cat1 = new Category { CategotyName = "Lamp" };
            //Category cat2 = new Category { CategotyName = "Roset" };
            //SubCategory sCat1 = new SubCategory { SubCategoryName = "LED", Category = cat1 };
            //SubCategory sCat2 = new SubCategory { SubCategoryName = "Filament", Category = cat1 };
            //Analog anal1 = new Analog { Name = "Lamp" };
            //Product p1 = new Product { Availability = true, Name = "8w", Price = 50.4d, SubCategory = sCat2, Discription = "Лампа 8вт" };
            //Product p2 = new Product { Availability = true, Name = "100w", Price = 10.12d, SubCategory = sCat1, Analog = anal1 };
            //Product p3 = new Product { Availability = true, Name = "50w", Price = 4.74, Manufacturer = "IEK", SubCategory = sCat1 };
            //Product p4 = new Product { Availability = true, Name = "1W", Price = 4.74, SubCategory = sCat1 };
            //Product p5 = new Product { Availability = true, Name = "2W", Price = 4.74, Articul = "47564", SubCategory = sCat1 };
            //Product p6 = new Product { Availability = true, Name = "3W", Price = 4.74, SubCategory = sCat1 };
            //Product p7 = new Product { Availability = true, Name = "4w", Price = 4.74, SubCategory = sCat1, Analog = anal1 };
            //Product p8 = new Product { Availability = true, Name = "5w лампа", Price = 4.74, SubCategory = sCat1 };
            //Product p9 = new Product { Name = "6w", Price = 4.74, SubCategory = sCat1 };
            //Product p10 = new Product { Name = "7w", Price = 4.74, SubCategory = sCat1 };
            //Categories.AddRange(cat1, cat2);
            //Analogs.AddRange(anal1);
            //SubCategories.AddRange(sCat1, sCat2);
            //Products.AddRange(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
            //SaveChanges();
            //Complect comp1 = new Complect { Name = "ComplectLamp" };
            //Complects.Add(comp1);
            //SaveChanges();
            //comp1.Products.Add(p2);
            //comp1.Products.Add(p5);
            //p2.isComplect = true;
            //p5.isComplect = true;
            //SaveChanges();

        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasMany(c => c.Orders)
                .WithMany(s => s.Products)
                .UsingEntity<CountToOrder>(
                   j => j
                    .HasOne(pt => pt.Order)
                    .WithMany(t => t.CountToOrder)
                    .HasForeignKey(pt => pt.OrderId),
                j => j
                    .HasOne(pt => pt.Product)
                    .WithMany(p => p.CountToOrder)
                    .HasForeignKey(pt => pt.ProductId),
                j =>
                {
                    j.Property(pt => pt.Count).HasDefaultValue(3);
                    j.HasKey(t => new { t.ProductId, t.OrderId });
                    j.ToTable("CountToOrder");
                });
        }
    }
}
