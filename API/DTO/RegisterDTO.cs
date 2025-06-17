using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace API.DTO;

public class RegisterDTO
{
    [Required]
    public string Email { get; set; } = string.Empty;


    [Required]

    public string Password { get; set; } = string.Empty;

    [Required]

    public string FirstName { get; set; } = string.Empty;


    [Required]

    public string LastName { get; set; } = string.Empty;


}
