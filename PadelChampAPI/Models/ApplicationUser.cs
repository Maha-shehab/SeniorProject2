﻿using Microsoft.AspNetCore.Identity;

namespace PadelChampAPI.Models;

public class ApplicationUser : IdentityUser
{

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    
    public string? Password { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }
}
