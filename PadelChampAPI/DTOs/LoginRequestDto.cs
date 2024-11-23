using System.ComponentModel.DataAnnotations;

namespace PadelChampAPI.DTOs;

public class LoginRequestDto
{
    [EmailAddress]
    [Required(ErrorMessage = "email is Required to login")]
    public string Email { get; set; }

    [Required(ErrorMessage = "password is Required to login")]
    public string Password { get; set; }


}
