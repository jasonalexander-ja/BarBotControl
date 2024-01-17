using System.ComponentModel.DataAnnotations;

namespace BarBotControl.Models;

public class UsernamePassword
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
