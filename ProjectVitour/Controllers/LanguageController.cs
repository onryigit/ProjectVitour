using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectVitour.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            // Case 3 madde 3: "bu seçim cookie tabanlı yönetilecektir."
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}