using DAS_DESAFIO_DOS_INVENTARIO.Controllers.Validations;
using DAS_DESAFIO_DOS_INVENTARIO.Servicios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace DAS_DESAFIO_DOS_INVENTARIO.Controllers
{
    public class AuthController : Controller
    {

        IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Signin()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirige al Home si el usuario ya está autenticado
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SigninAsync(InputSignin inputSignin)
        {

            if (ModelState.IsValid)
            {
                Debug.WriteLine($"{inputSignin.Email} => {inputSignin.Password}");
                var signinResult = _authService.Signin(inputSignin.Email, inputSignin.Password);
                Debug.WriteLine($"Signin result => {signinResult} - {signinResult.Role.Name}");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, inputSignin.Email),
                    new Claim(ClaimTypes.Role, signinResult.Role.Name),
                };

                var claimsIdentity = new ClaimsIdentity(
                                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60 * 8),


                    IsPersistent = true,


                    IssuedUtc = DateTimeOffset.Now,
                    // The time at which the authentication ticket was issued.

                    RedirectUri = "/Home/Privacy"
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.


                };

                await HttpContext.SignInAsync(
                      CookieAuthenticationDefaults.AuthenticationScheme,
                      new ClaimsPrincipal(claimsIdentity),
                      authProperties);
                return LocalRedirect("/Home/Index");

            }

            return View(inputSignin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Signin", "Auth");
        }
        public IActionResult Forbidden()
        {

            return View();
        }

    }
}
