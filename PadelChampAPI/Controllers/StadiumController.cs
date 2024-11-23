using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PadelChampAPI.Interfaces;
using PadelChampAPI.Models;

namespace PadelChampAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StadiumController(IGenericRepository<PadelPlayground> repository) : ControllerBase
{
    public async Task<IActionResult> Index()
    {
        return Ok(await repository.GetAll());
    }
}
