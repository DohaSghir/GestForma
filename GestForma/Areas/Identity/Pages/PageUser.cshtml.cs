using GestForma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestForma.Areas.Identity.Pages
{
    [Authorize]
    public class PageUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
       public ApplicationUser? user;

        public PageUserModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public void OnGet()
        {

            var task = userManager.GetUserAsync(User);
            task.Wait();
             user = task.Result;
     

        }
    }
}
