using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : ControllerBase
    {
        private readonly string username = "assem.mohammed@live.com";
        private readonly string password = "Dubai123@@";
        [HttpPost("Register")]
        public async Task<IActionResult> Register()
        {
            var result = await userManager.CreateAsync(new IdentityUser
            {
                UserName = username,
                Email = username,
                TwoFactorEnabled = true,
            }, password);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login()
        {
            IdentityUser user = (await userManager.FindByNameAsync(username))!;

            _ = await signInManager.PasswordSignInAsync(user, password, false, false);

            await userManager.UpdateSecurityStampAsync(user);
            var DefaultProvider = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultProvider);
            var DefaultEmailProvider = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            var DefaultPhoneProvider = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
            var DefaultAuthenticatorProvider = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider);

            return Ok(new Tokens
            {
                DefaultProvider = DefaultProvider,
                DefaultEmailProvider = DefaultEmailProvider,
                DefaultPhoneProvider = DefaultPhoneProvider,
                DefaultAuthenticatorProvider = DefaultAuthenticatorProvider
            });
        }

        [HttpPost("ValidateTFA")]
        public async Task<IActionResult> ValidateTwoFactorAuthentication(Tokens tokens)
        {
            var user = (await userManager.FindByNameAsync(username))!;

            var response = new TokensResponse
            {
                DefaultProvider = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultProvider, tokens.DefaultProvider),
                DefaultEmailProvider = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, tokens.DefaultEmailProvider),
                DefaultPhoneProvider = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, tokens.DefaultPhoneProvider),
                DefaultAuthenticatorProvider = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, tokens.DefaultAuthenticatorProvider)
            };

            await userManager.UpdateSecurityStampAsync(user);

            return Ok(response);
        }
    }

    public class Tokens
    {
        public string DefaultProvider { get; set; } = default!;
        public string DefaultEmailProvider { get; set; } = default!;
        public string DefaultPhoneProvider { get; set; } = default!;
        public string DefaultAuthenticatorProvider { get; set; } = default!;
    }

    public class TokensResponse
    {
        public bool DefaultProvider { get; set; } = default!;
        public bool DefaultEmailProvider { get; set; } = default!;
        public bool DefaultPhoneProvider { get; set; } = default!;
        public bool DefaultAuthenticatorProvider { get; set; } = default!;
    }
}
