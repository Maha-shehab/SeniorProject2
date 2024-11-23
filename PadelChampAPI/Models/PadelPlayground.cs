using System.ComponentModel.DataAnnotations;

namespace PadelChampAPI.Models;

public class PadelPlayground
{
    [Key]
    public int Id { get; set; } 

    [Required]
    public string? PlaygroundName { get; set; }

    [Required]
    public string? Coordinates { get; set; }

}

/*
    
Name :Padel Pin            Coordinates: 21.649754,39.1312476


Name: Deem Padel     Coordinates:21.6045004,39.1122674


Name :PW padel world Coordinates : 21.7472331,39.1364855


Name:Padelhood Coordinates:21.7496125,39.1351022


Name :The padel place     Coordinates :21.6596464,39.1161693


Name: Padel-L         Coordinates:21.5647671,39.1459853


Name : Just Padel  Coordinates : 21.7476718,39.1369144


Name :Padel in Coordinates : 21.7130623,39.1009422


Name : Padel Cube Coordinates :21.6518133,39.1269861


Name : Padel Royale  Coordinates:21.7477969,39.1359017
 */
