using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Token;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _user;
    private readonly SignInManager<ApplicationUser> _sign;

    public AccountController(UserManager<ApplicationUser> user, SignInManager<ApplicationUser> sign)
    {
        _user = user;
        _sign = sign;
    }

    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("CreateToken")]
    public async Task<IActionResult> CreateToken([FromBody]InputModel model)
    {
        if (!ModelState.IsValid)
        {
            return Unauthorized();
        }

        var result = await _sign.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var token = new TokenJwtBuilder()
                .AddSecurityKey(JwtSecurityKey.Create("FinalcialCore_Secret_Key-20232004"))
                .AddSubject("Financial Core v1")
                .AddIssuer("FinacialCore.Security.Bearer")
                .AddAudience("FinacialCore.Security.Bearer")
                .AddClaim("UsuarioEmail", model.Email)
                .AddExpiry(5)
                .Builder();

            return Ok(token.value);
        }
        else
            return Unauthorized();
    }
}
