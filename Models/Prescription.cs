using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        public int MedicalRecordId { get; set; }

        public string? MedicationName { get; set; }
        public string? Dosage { get; set; }
        public int Frequency { get; set; } // Number of times per day
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? SpecialInstructions { get; set; }

        // العلاقة
        public MedicalRecord? MedicalRecord { get; set; }
    }
}