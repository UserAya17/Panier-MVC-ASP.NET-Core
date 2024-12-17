namespace MyShop.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // FK
        public Product Product { get; set; } // Navigation property
        public int? Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal? Total { get; set; }
    }
}
