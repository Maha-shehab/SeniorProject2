using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PadelChampAPI.Data;
using PadelChampAPI.Models;

namespace PadelChampAPI.Services;

public class AuthenticatoinService : ICustomAuthenticationService
{
    private readonly IJwtTokenGenerator JwtTokenGenerator;
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatoinService(IHttpContextAccessor httpContextAccessor, AppDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _userManager = userManager;
        _config = config;
        JwtTokenGenerator = jwtTokenGenerator;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<Response<string>> Register(string UserName, string Email, string Password, string PhoneNumber, string FirstName, string LastName, string Gender, DateTime DateOfBirth)
    {
        var userExist = await _userManager.FindByEmailAsync(Email);

        var response = new Response<string>();

        if (userExist != null)
        {
            response.Status = "Failed";
            response.isError = true;
            response.Errors.Add("This email is already registered");
            return response;
        }

        var user = new ApplicationUser
        {
            UserName = UserName,
            Email = Email,
            PasswordHash = Password,
            FirstName = FirstName,
            LastName = LastName,
            PhoneNumber = PhoneNumber,
            Gender = Gender,
            DateOfBirth = DateOfBirth,
            Password=Password
        };

        var result = await _userManager.CreateAsync(user, Password);

        if (!result.Succeeded)
        {
            response.Status = "Failed";
            response.isError = true;
            response.Errors = result.Errors.Select(error => error.Description).ToList();
            return response;
        }

        var claims = new List<Claim>
    {
        new Claim("userId", user.Id)
    };

        var tokenString = JwtTokenGenerator.GenerateToken(claims, _config);


        response.Message = $"User Registered Successfully with Email: {user.Email}";
        response.Payload = tokenString;

        return response;
    }

    private void SetAuthenticationCookie(string tokenString)
    {
        _httpContextAccessor.HttpContext?.Response.Headers.Add("x-Authorization", $"Bearer {tokenString}");

        // Set the token in a cookie
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("Auth", tokenString, GetCookieOptions());
    }

    private CookieOptions GetCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddMinutes(120)
        };
    }


    public async Task<Response<object>> Login(string Email, string Password)
    {

        var user = await _userManager.FindByEmailAsync(Email);

        var isValidPassword = user != null && await _userManager.CheckPasswordAsync(user,Password);

        var response = new Response<object>();

        if (user is null || !isValidPassword)
        {
            response.Status = "Failed";
            response.isError = true;
            response.Errors.Add("Invalid user Name or Password");
            return response;
        }


        List<Claim> claims = new()
        {
            new Claim("userId", user.Id),
            new Claim("Email",user.Email!),
            new Claim("Name",user.FirstName!)
        };

        var tokenString = JwtTokenGenerator.GenerateToken(claims, _config);
        SetAuthenticationCookie(tokenString);


        var payload = new
        {
            JwtToken=tokenString,
            UserId=user.Id,
            Email = user.Email!,
            Name = user.FirstName!,
        };

        response.Message = "User logged in Successfully";
        response.Payload = payload;
        return response;
    }

}
