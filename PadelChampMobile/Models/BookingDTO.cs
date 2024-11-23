namespace PadelChampMobile.Models;

public class BookingDTO
{
    public int? Price { get; set; }
    public int? Duration {  get; set; }
    public string? StartTime {  get; set; }
    public int? PadelPlaygroundId {  get; set; }
    public string? UserId {  get; set; }
    public DateTime? BookingDate {  get; set; }
    public BookingDTO()
    {

    }
    public bool isBookingComplete => Price != null &&
        Duration != null &&
        StartTime != null &&
        PadelPlaygroundId != null &&
        UserId != null &&
        BookingDate != null;
    public override string ToString()
    => $"UserId: {UserId}\nPlaygroundId: {PadelPlaygroundId}\nPrice: {Price}\nDuration: {Duration}" +
        $"\nStartTime: {StartTime}\nBookingDate: {BookingDate}";
}
