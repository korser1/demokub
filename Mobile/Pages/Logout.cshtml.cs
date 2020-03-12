using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mobile.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task OnGet()
        {
                await HttpContext.SignOutAsync(Constants.OpenIdScheme, new AuthenticationProperties { RedirectUri = "/" });
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
