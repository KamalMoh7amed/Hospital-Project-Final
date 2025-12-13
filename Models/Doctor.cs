
namespace Hospital_Project.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public string? DoctorName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Specialization { get; set; }
        public char Gender { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}