using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.DTO;
using WebAppRazor.ORM;

namespace WebAppRazor.Pages
{
    public class RegisterCustomerModel : PageModel
    {
        private readonly AppDBContext _db;

        public RegisterCustomerModel(AppDBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public CustomerDTO Customer { get; set; }

        public string? Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.customers.Add(Customer);
            _db.SaveChanges();

            Message = "Customer registered successfully.";
            Customer = new CustomerDTO();
            return Page();
        }
    }
}
