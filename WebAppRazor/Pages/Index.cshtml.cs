using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.DTO;
using WebAppRazor.ORM;

namespace WebAppRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDBContext _appDbContext;

        public IndexModel(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IList<ProductDTO> products { get; set; }

        public void OnGet()
        {
            products = _appDbContext.products.ToList();
        }
    }
}
