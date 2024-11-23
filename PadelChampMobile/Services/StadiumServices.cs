using Microsoft.Maui.ApplicationModel.Communication;
using Newtonsoft.Json;
using PadelChampMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelChampMobile.Services;

public class StadiumServices : IStadiumServices
{
    public async Task<IEnumerable<PadelStadiumViewModel>?> GetAll()
    {
        using var client = new HttpClient();
        string baseUrl = "http://10.0.2.2:5079/api/stadium";
        client.BaseAddress = new Uri(baseUrl);

        // Make the GET request
        HttpResponseMessage response = await client.GetAsync("");
        if (!response.IsSuccessStatusCode)
            return null;


        var jsonResponse = await response.Content.ReadAsStringAsync();
        var stadiums = JsonConvert.DeserializeObject<List<PadelStadiumViewModel>>(jsonResponse);
        return stadiums;

    }

}
