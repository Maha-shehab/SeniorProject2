using System.ComponentModel.DataAnnotations;

namespace PadelChampAPI.DTOs;

public class AvailableBookingDTO
{
    public TimeSpan StartTime { get; set; }
    public List<int> Durations { get; set; } = null!;  
}
