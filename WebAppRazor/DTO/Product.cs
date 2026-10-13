using System.ComponentModel.DataAnnotations;

namespace WebAppRazor.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        // Allow decimal prices and require a value
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public decimal PriceTotal { get; set; }

        public string Description { get; set; } = string.Empty;

        [Timestamp]   // Marks this column as a concurrency token
        public byte[] RowVersion { get; set; }
    }
}
