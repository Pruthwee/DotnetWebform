using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShopLegacyWebForms.Pages
{
    public class ErrorModel : PageModel
    {
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = "An error occurred while processing your request.";
        }
    }
}
