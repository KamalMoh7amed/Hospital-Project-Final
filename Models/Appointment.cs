using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateOnly AppointmentDate { get; set; }
        public TimeOnly Time { get; set; }
        public string? AppointmentStatus { get; set; }

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}