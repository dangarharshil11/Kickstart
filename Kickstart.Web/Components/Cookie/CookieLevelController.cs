using CMS.Helpers;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Kickstart.Web.Components.Cookie
{
    public class CookieLevelController : Controller
    {
        private readonly ICurrentCookieLevelProvider cookieLevelService;
        private readonly ICookieAccessor cookieAccessor;

        // Initializes instances of required services using dependency injection
        public CookieLevelController(ICurrentCookieLevelProvider cookieLevelService, ICookieAccessor cookieAccessor)
        {
            this.cookieLevelService = cookieLevelService;
            this.cookieAccessor = cookieAccessor;
        }

        [Route("Cookie")]
        public ActionResult Index()
        {
            // Creates a list with the three default levels of cookie compliance relevant for visitors.
            // The CookieLevel class is used to directly extract integer values corresponding to each cookie level
            // so that no additional mapping is necessary.
            var cookieLevels = new List<object>() {
                new { label = "Essential", value = Kentico.Web.Mvc.CookieLevel.Essential.Level },
                new { label = "Visitor", value = Kentico.Web.Mvc.CookieLevel.Visitor.Level },
                new { label = "All", value = Kentico.Web.Mvc.CookieLevel.All.Level }
            };

            return View(new SelectList(cookieLevels, "value", "label"));
        }

        // Sets the cookie level for the current user
        // The value passed into the action from the corresponding view directly corresponds to one of the system's
        // cookie levels thanks to the CookieLevel class.
        [HttpPost("SetCookieLevel")]
        [ValidateAntiForgeryToken]
        public ActionResult SetCookieLevel(int selectedValue)
        {
            // Sets the cookie level for the current user to the level corresponding to the provided value
            cookieLevelService.SetCurrentCookieLevel(selectedValue);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost("Set")]
        [ValidateAntiForgeryToken]
        public ActionResult Set()
        {
            cookieAccessor.Set("CustomCookie", "CustomValue", new CookieOptions() { SameSite = SameSiteMode.None, Secure = true });
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult Remove()
        {
            cookieAccessor.Remove("CustomCookie");
            return RedirectToAction(nameof(Index));
        }
    }
}
