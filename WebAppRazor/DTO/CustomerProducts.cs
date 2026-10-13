namespace WebAppRazor.DTO
{
    public class CustomerProductsDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public  decimal Price { get; set; }

        public  DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
