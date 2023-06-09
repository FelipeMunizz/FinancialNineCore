﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs;

public class LoginDTO
{
    [Required (ErrorMessage = "Informe email")]
    [EmailAddress]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Informe senha")]
    public string? Password { get; set; }
    [Required(ErrorMessage = "Informe CPF")]
    public string? CPF { get; set; }
}
