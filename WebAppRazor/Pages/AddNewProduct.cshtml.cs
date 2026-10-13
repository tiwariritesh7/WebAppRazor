using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.DTO;
using WebAppRazor.ORM;

namespace WebAppRazor.Pages
{
    public class AddNewProductModel : PageModel
    {
        private readonly AppDBContext _db;

        public AddNewProductModel(AppDBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ProductDTO Product { get; set; } = new ProductDTO();

        [TempData]
        public string? Message { get; set; }

        public void OnGet()
        {
            // Message (if any) is loaded from TempData automatically via the TempData attribute
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
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
