using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs;

public class EditUserDTO
{
    [Required(ErrorMessage = "Informe email")]
    [EmailAddress]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Informe CPF")]
    public string? CPF { get; set; }
}
