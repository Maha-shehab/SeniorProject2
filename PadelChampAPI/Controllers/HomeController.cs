using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PadelChampAPI.Interfaces;
using PadelChampAPI.Models;

namespace PadelChampAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IGenericRepository<PadelPlayground> repository) : ControllerBase
{
    [HttpGet("/")]
    public IActionResult sayHello()
    {
        return Ok("This works!");
    }
    public async Task<IActionResult> Index()
    {
        var Playgrounds = new List<PadelPlayground>
            {
                new PadelPlayground { PlaygroundName = "Padel Pin", Coordinates = "21.649754,39.1312476" },
                new PadelPlayground { PlaygroundName = "Deem Padel", Coordinates = "21.6045004,39.1122674" },
                new PadelPlayground { PlaygroundName = "PW padel world", Coordinates = "21.7472331,39.1364855" },
                new PadelPlayground { PlaygroundName = "Padelhood", Coordinates = "21.7496125,39.1351022" },
                new PadelPlayground { PlaygroundName = "The padel place", Coordinates = "21.6596464,39.1161693" },
                new PadelPlayground { PlaygroundName = "Padel-L", Coordinates = "21.5647671,39.1459853" },
                new PadelPlayground { PlaygroundName = "Just Padel", Coordinates = "21.7476718,39.1369144" },
                new PadelPlayground { PlaygroundName = "Padel in", Coordinates = "21.7130623,39.1009422" },
                new PadelPlayground { PlaygroundName = "Padel Cube", Coordinates = "21.6518133,39.1269861" },
                new PadelPlayground { PlaygroundName = "Padel Royale", Coordinates = "21.7477969,39.1359017" }
            };
        Playgrounds.ForEach(playground => repository.Create(playground));
        await repository.SaveChangesAsync();

        return Ok("This works!");
    }
    
}
