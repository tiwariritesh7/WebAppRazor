using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAppRazor.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        // Price stored as integer in the database (matches current schema)
        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        public decimal PriceTotal { get; set; }

        public string Description { get; set; } = string.Empty;

        [BindNever]
        [Timestamp]   // Marks this column as a concurrency token
        public byte[] RowVersion { get; set; }
    }
}
