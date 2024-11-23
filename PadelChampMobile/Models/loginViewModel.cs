using System.ComponentModel.DataAnnotations;

namespace PadelChampMobile.Models;

public class loginViewModel
{
    [EmailAddress]
    [Required(ErrorMessage = "email is Required to login")]
    public string Email { get; set; }

    [Required(ErrorMessage = "password is Required to login")]
    public string Password { get; set; }


}
