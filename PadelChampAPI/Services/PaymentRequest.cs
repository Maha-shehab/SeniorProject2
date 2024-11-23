using PadelChampAPI.Models;

namespace PadelChampAPI.Services;
public class PaymentRequest
{
    public int Amount  { get; set; }
    public string Currency { get; set; } = "sar"; // Default currency
    public string Description { get; set; }
    public string Email { get; set; }

    // Custom properties
    public Booking booking { get; set; }
}