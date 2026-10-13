using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppRazor.DTO;
using WebAppRazor.ORM;

namespace WebAppRazor.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public CustomerProductsDTO CustomerProducts { get; set; }
        [BindProperty]
        public int SelectedCustomerId { get; set; }

        [BindProperty]
        public int SelectedProductId { get; set; }

        [BindProperty]
        public decimal Price { get; set; }
        private readonly AppDBContext _appDbContext;

        public IndexModel(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IList<CustomerProductsDTO> customerProducts { get; set; }
        public SelectList Products { get; set; }
        public SelectList Customers { get; set; }
        public IDictionary<int, string> ProductImages { get; set; } = new Dictionary<int, string>();

        public void OnGet()
        {
            customerProducts = _appDbContext.customersProducts.ToList();
            Products = new SelectList(_appDbContext.products.ToList(), "Id", "Name");
            Customers = new SelectList(_appDbContext.customers.ToList(), "Id", "Name");
            ProductImages = _appDbContext.products.ToDictionary(p => p.Id, p => string.IsNullOrEmpty(p.ImagePath) ? "placeholder.png" : p.ImagePath);
        }

        public IActionResult OnPost()
        {
            // re-populate selects
            Products = new SelectList(_appDbContext.products.ToList(), "Id", "Name");
            Customers = new SelectList(_appDbContext.customers.ToList(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                customerProducts = _appDbContext.customersProducts.ToList();
                return Page();
            }

            var product = _appDbContext.products.FirstOrDefault(p => p.Id == SelectedProductId);
            var customer = _appDbContext.customers.FirstOrDefault(c => c.Id == SelectedCustomerId);

            var entry = new CustomerProductsDTO
            {
                ProductId = SelectedProductId,
                ProductName = product?.Name ?? string.Empty,
                Price = Price != 0 ? Price : (product != null ? Convert.ToDecimal(product.Price) : 0m),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Name = customer?.Name ?? string.Empty
            };

            _appDbContext.customersProducts.Add(entry);
            _appDbContext.SaveChanges();

            ProductImages = _appDbContext.products.ToDictionary(p => p.Id, p => string.IsNullOrEmpty(p.ImagePath) ? "placeholder.png" : p.ImagePath);

            return RedirectToPage();
        }
    }
}
