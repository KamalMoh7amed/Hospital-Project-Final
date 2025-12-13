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
        public int Frequency { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? SpecialInstructions { get; set; }


        public MedicalRecord? MedicalRecord { get; set; }
    }
}