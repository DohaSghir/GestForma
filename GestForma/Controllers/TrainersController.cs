using GestForma.Models;
using GestForma.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text;

namespace GestForma.Controllers
{
    public class TrainersController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<TrainersController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public TrainersController(
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<TrainersController> logger,
            IEmailSender emailSender)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
            _emailStore = userStore as IUserEmailStore<ApplicationUser>
                ?? throw new InvalidCastException("The provided IUserStore is not an IUserEmailStore.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }


        public IActionResult AddTrainer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(TrainerRegister model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                // If the model state is invalid, redisplay the form
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // Assign default role
                await _userManager.AddToRoleAsync(user, "invité");

                // Generate email confirmation token
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Build confirmation link
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                new { userId, code, returnUrl },
                    protocol: Request.Scheme);

                // Send confirmation email
                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                );

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToAction("RegisterConfirmation", new { email = model.Email, returnUrl });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }

            // Add errors to ModelState for display in the view
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Redisplay form if we got this far
            return View(model);
        }

    }
}
