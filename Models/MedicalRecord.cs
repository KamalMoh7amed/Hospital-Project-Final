using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Models
{
    public class MedicalRecord
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }

        public string? Description { get; set; }
        public string? Diagnosis { get; set; }
        public string? AdditionalNote { get; set; }

        // العلاقات
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public Appointment? Appointment { get; set; }
        public Prescription? Prescription { get; set; }
    }
}