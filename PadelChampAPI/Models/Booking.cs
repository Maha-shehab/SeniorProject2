using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadelChampAPI.Models;

public class Booking
{
    [Key]
    public int BookingId { get; set; }

    [Required]
    public string UserId { get; set; } = null!; // Foreign Key to User

    [ForeignKey("UserId")]  // Reference to the foreign key property
    public ApplicationUser? User { get; set; }  // Navigation property for User

    [Required]
    public int PadelPlaygroundId { get; set; }  // Foreign Key to Padel Playground

    [ForeignKey("PadelPlaygroundId")]  // Reference to the foreign key property
    public PadelPlayground? PadelPlayground { get; set; }  // Navigation property for Playground

    [Required]
    public DateTime BookingDate { get; set; }  // Date of the booking

    [Required]
    public TimeSpan StartTime { get; set; }  // Start time of the booking

    [Required]
    [Range(60, 120)]  // Duration can only be 60, 90, or 120 minutes
    public int Duration { get; set; }  // Duration in minutes

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    
    public int Price {  get; set; } 
}
