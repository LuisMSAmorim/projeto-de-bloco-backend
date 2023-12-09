using System.ComponentModel.DataAnnotations;

namespace FisioFinancials.Domain.Model.DTOs;

public class LoginUserDTO
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
