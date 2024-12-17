using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial data for Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Brand = "Trek",
                    Model = "FX 1",
                    Year = 2022,
                    Type = "Hybrid",
                    Price = 450.00m,
                    Color = "Black",
                    ImageFileName = "trek-fx1-black.jpg"
                },
                new Product
                {
                    Id = 2,
                    Brand = "Giant",
                    Model = "TCR Advanced Pro 1 Disc",
                    Year = 2023,
                    Type = "Road",
                    Price = 3500.00m,
                    Color = "Red",
                    ImageFileName = "giant-tcr-red.jpg"
                },
                new Product
                {
                    Id = 3,
                    Brand = "Specialized",
                    Model = "Rockhopper Expert 29",
                    Year = 2022,
                    Type = "Mountain",
                    Price = 1100.00m,
                    Color = "Blue",
                    ImageFileName = "specialized-rockhopper-blue.jpg"
                },
                new Product
                {
                    Id = 4,
                    Brand = "Cannondale",
                    Model = "Synapse Carbon 105",
                    Year = 2023,
                    Type = "Road",
                    Price = 2000.00m,
                    Color = "White",
                    ImageFileName = "cannondale-synapse-white.jpg"
                },
                new Product
                {
                    Id = 5,
                    Brand = "Trek",
                    Model = "Marlin 5",
                    Year = 2022,
                    Type = "Mountain",
                    Price = 550.00m,
                    Color = "Green",
                    ImageFileName = "trek-marlin-green.jpg"
                }
            );
        }
    }
}
