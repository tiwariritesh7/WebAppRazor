using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;
using WebAppRazor.DTO;
using WebAppRazor.ORM;

namespace WebAppRazor.Pages
{
    public class AddNewProductModel : PageModel
    {
        private readonly AppDBContext _db;
        private readonly IWebHostEnvironment _env;

        public AddNewProductModel(AppDBContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [BindProperty]
        public ProductDTO Product { get; set; } = new ProductDTO();

        [BindProperty]
        public IFormFile? ProductImage { get; set; }

        [TempData]
        public string? Message { get; set; }

        public void OnGet()
        {
            // Message (if any) is loaded from TempData automatically via the TempData attribute
        }

        public async Task<IActionResult> OnPost()
        {
            // Remove any RowVersion model state entries coming from form inputs to avoid false validation failures
            var keysToRemove = ModelState.Keys.Where(k => k != null && k.Contains("RowVersion")).ToList();
            foreach (var k in keysToRemove)
            {
                ModelState.Remove(k);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (ProductImage != null && ProductImage.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "images", "products");
                    Directory.CreateDirectory(uploads);
                    var ext = Path.GetExtension(ProductImage.FileName);
                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var filePath = Path.Combine(uploads, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProductImage.CopyToAsync(stream);
                    }
                    Product.ImagePath = fileName;
                }

                _db.products.Add(Product);
                _db.SaveChanges();

                Message = "Product added successfully.";

                // Post-Redirect-Get to avoid duplicate submissions and clear the form
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // Log exception as needed; for now surface a model error
                ModelState.AddModelError(string.Empty, "An error occurred while saving the product.");
                return Page();
            }
        }
    }
}
