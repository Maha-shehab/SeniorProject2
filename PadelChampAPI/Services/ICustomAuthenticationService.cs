using PadelChampAPI.DTOs;
using PadelChampAPI.Data;


namespace PadelChampAPI.Services;

public interface ICustomAuthenticationService
{
    Task<Response<string>> Register(string UserName, string Email, string Password, string PhoneNumber, string FirstName, string LastName, string Gender, DateTime DateOfBirth);

    Task<Response<Object>> Login(string Email, string Password);

}
