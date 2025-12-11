using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Models
{
    public class Payments
    {
        [Key]
        public int Id { get; set; }

        public int AppointmentId { get; set; } // Foreign Key

        public DateOnly PaymentDate { get; set; }
        public string? PaymentMethod { get; set; } // مثلاً: نقدًا، فيزا، تحويل...
        public decimal AmountPaid { get; set; }
        public string? AdditionalNote { get; set; }

        // العلاقة
        public Appointment? Appointment { get; set; }
    }
}