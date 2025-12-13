using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        public string? PatientName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public char Gender { get; set; } 
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public Appointment? Appointment { get; set; }
        public MedicalRecord? medmedicalRecords { get; set; }
    }
}