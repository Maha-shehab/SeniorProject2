using PadelChampMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelChampMobile.Services
{
    public interface IregisterRepository
    {
        Task<string> Register(string UserName, string Email, string Password, string PhoneNumber, string FirstName, string LastName, string Gender, DateTime DateOfBirth);

    }
}
