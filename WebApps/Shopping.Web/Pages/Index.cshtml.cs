using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Cataloge;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
	public class IndexModel(ICatalogService service, ILogger<IndexModel> logger) : PageModel
	{
		public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();		
		public async Task<IActionResult> OnGetAsync()
		{
			logger.LogInformation("Index Page Visited");
			var result = await service.GetProducts();
			ProductList = result.Products;
			return Page();
		}
	}
}
