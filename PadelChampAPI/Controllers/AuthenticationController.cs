using Microsoft.AspNetCore.Mvc;
using PadelChampAPI.Services;

namespace PadelChampAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ICustomAuthenticationService _authenticationService;

    public AuthenticationController(ICustomAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet]
    [Route("login/{Email}/{Password}")]
    public async Task<IActionResult> Login(string Email, string Password)
    {
        var response = await _authenticationService.Login(Email, Password);
        Console.WriteLine("Hello from the login method!");
        if (response.isError)
        {
            return Unauthorized(response);
        }

        // Add the token to the headers
        return Ok(response);
    }

    //used to define the routing behavior for an action method.
    [HttpGet]
    [Route(
        "register/{UserName}/{Email}/{Password}/{PhoneNumber}/{FirstName}/{LastName}/{Gender}/{DateOfBirth}"
    )]
    public async Task<IActionResult> Register(
        string UserName,
        string Email,
        string Password,
        string PhoneNumber,
        string FirstName,
        string LastName,
        string Gender,
        DateTime DateOfBirth
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); //404 for bad request
        }

        var response = await _authenticationService.Register(
            UserName,
            Email,
            Password,
            PhoneNumber,
            FirstName,
            LastName,
            Gender,
            DateOfBirth
        );

        if (response.isError)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}
