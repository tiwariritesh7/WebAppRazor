using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppRazor.DTO;
using WebAppRazor.ORM;

namespace WebAppRazor.Pages
{
    public class SaleProductToCustomerModel : PageModel
    {
        private readonly AppDBContext _db;

        public SaleProductToCustomerModel(AppDBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public int SelectedCustomerId { get; set; }

        [BindProperty]
        public int SelectedProductId { get; set; }

        [BindProperty]
        public decimal SalePrice { get; set; }

        public SelectList Customers { get; set; }

        public SelectList Products { get; set; }

        public string? Message { get; set; }

        public void OnGet()
        {
            Customers = new SelectList(_db.customers.ToList(), "Id", "Name");
            Products = new SelectList(_db.products.ToList(), "Id", "Name");
        }

        public IActionResult OnPost()
        {
            Customers = new SelectList(_db.customers.ToList(), "Id", "Name");
            Products = new SelectList(_db.products.ToList(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var product = _db.products.FirstOrDefault(p => p.Id == SelectedProductId);
            if (product == null)
            {
                ModelState.AddModelError(string.Empty, "Selected product not found.");
                return Page();
            }

            var sale = new CustomerProductsDTO
            {
                ProductId = SelectedProductId,
                ProductName = product.Name,
                Price = SalePrice != 0 ? SalePrice : product.Price,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Name = _db.customers.FirstOrDefault(c => c.Id == SelectedCustomerId)?.Name ?? string.Empty
            };

            _db.customersProducts.Add(sale);
            _db.SaveChanges();

            Message = "Product sold to customer successfully.";
            return Page();
        }
    }
}
