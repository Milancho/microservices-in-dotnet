using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwoFactorAuthenticationGoogleAuthenticatorAngular.DataTransferObjects;

namespace TFAGoogleAuthenticatorAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public Task<IActionResult> Get()
        {
            return Task.FromResult<IActionResult>(Ok());
        }
    }
}
