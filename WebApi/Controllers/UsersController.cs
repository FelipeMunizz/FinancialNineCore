using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebApi.DTOs;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _user;
    private readonly SignInManager<ApplicationUser> _sign;

    public UsersController(UserManager<ApplicationUser> user, SignInManager<ApplicationUser> sign)
    {
        _user = user;
        _sign = sign;
    }

    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] LoginDTO login)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ApplicationUser user = new ApplicationUser
        {
            Email = login.Email,
            UserName = login.Email,
            CPF = login.CPF
        };

        IdentityResult result = await _user.CreateAsync(user, login.Password);

        if (result.Errors.Any())
        {
            return BadRequest(result.Errors);
        }

        // Autenticacao dois fatores por email
        string code = await _user.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        //retorno do email
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        var responseRetorn = await _user.ConfirmEmailAsync(user, code);
        if (responseRetorn.Succeeded)
            return Ok("Usuario Adicionado");
        else
            return BadRequest("Erro ao confirmar cadastro");
    }
}
