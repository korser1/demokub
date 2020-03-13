using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mobile.Pages
{
    public class LoginModel : PageModel
    {
        public async Task OnGet(string redirectUri)
        {
            await HttpContext.ChallengeAsync(Constants.OpenIdScheme, new AuthenticationProperties
            {
                RedirectUri = redirectUri
            });
        }
    }
}
