using System.ComponentModel.DataAnnotations;

namespace PadelChampMobile.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "UserName is Required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "FirstName is Required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "LastName is Required")]
    public string LastName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is Required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is Required")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
    [Required(ErrorMessage = "Confirm Password is Required")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Phone number is Required")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Gender is Required")]
    [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either Male or Female.")]
    public string Gender { get; set; }

    [Required(ErrorMessage = "Date of Birth is Required")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
}
