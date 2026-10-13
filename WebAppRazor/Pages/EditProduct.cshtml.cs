using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAppRazor.DTO;
using WebAppRazor.ORM;

namespace WebAppRazor.Pages
{
    public class EditProductModel : PageModel
    {
        private readonly AppDBContext _context;

        public EditProductModel(AppDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductDTO Product { get; set; }

        // Bind a base64 representation of the RowVersion to avoid byte[] model binding issues
        [BindProperty]
        public string RowVersionBase64 { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.products.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            RowVersionBase64 = Product.RowVersion != null ? Convert.ToBase64String(Product.RowVersion) : string.Empty;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var productToUpdate = await _context.products.FindAsync(id);

            if (productToUpdate == null)
            {
                return NotFound();
            }

            // Copy new values from form
            productToUpdate.Name = Product.Name;
            productToUpdate.Price = Product.Price;

            // Handle concurrency with RowVersion (use base64 string from the form)
            if (!string.IsNullOrEmpty(RowVersionBase64))
            {
                try
                {
                    _context.Entry(productToUpdate).Property("RowVersion").OriginalValue = Convert.FromBase64String(RowVersionBase64);
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("RowVersion", "Invalid concurrency token value.");
                    return Page();
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (ProductDTO)entry.Entity;

                var databaseEntry = await entry.GetDatabaseValuesAsync();
                if (databaseEntry == null)
                {
                    ModelState.AddModelError("", "The product was deleted by another user.");
                }
                else
                {
                    var databaseValues = (ProductDTO)databaseEntry.ToObject();

                    ModelState.AddModelError("", "The record you attempted to edit "
                        + "was modified by another user after you got the original value. "
                        + "Your changes were not saved.");

                    // Refresh RowVersion so user can retry
                    Product.RowVersion = (byte[])databaseValues.RowVersion;
                    RowVersionBase64 = Product.RowVersion != null ? Convert.ToBase64String(Product.RowVersion) : string.Empty;
                }
            }

            return Page();
        }
    }
}
