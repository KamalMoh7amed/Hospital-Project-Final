
namespace Hospital_Project.Models
{
    public class Payments
    {
        [Key]
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public DateOnly PaymentDate { get; set; }
        public string? PaymentMethod { get; set; } 
        public decimal AmountPaid { get; set; }
        public string? AdditionalNote { get; set; }

       
        public Appointment? Appointment { get; set; }
    }
}