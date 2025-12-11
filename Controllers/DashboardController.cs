using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital_Project.Data;
using Hospital_Project.Models;

namespace Hospital_Project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // إحصائيات
            ViewBag.TotalPatients = await _context.Patients.CountAsync();
            ViewBag.TotalDoctors = await _context.Doctors.CountAsync();
            ViewBag.TotalAppointments = await _context.Appointments.CountAsync();
            ViewBag.TotalPayments = await _context.Payments.CountAsync();
            ViewBag.TotalRecords = await _context.MedicalRecords.CountAsync();

            // المواعيد اليوم
            var today = DateOnly.FromDateTime(DateTime.Today);
            ViewBag.TodayAppointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate == today)
                .ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SearchPatient(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                TempData["Error"] = "يرجى إدخال اسم المريض للبحث.";
                return RedirectToAction(nameof(Index));
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientName.Contains(searchTerm));

            if (patient == null)
            {
                TempData["Error"] = $"لا يوجد مريض باسم: {searchTerm}";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Patient = patient;
            ViewBag.Appointments = await _context.Appointments
                .Where(a => a.PatientId == patient.Id)
                .Include(a => a.Doctor)
                .ToListAsync();
            ViewBag.MedicalRecords = await _context.MedicalRecords
                .Where(mr => mr.PatientId == patient.Id)
                .Include(mr => mr.Doctor)
                .Include(mr => mr.Prescription)
                .ToListAsync();
            ViewBag.Payments = await _context.Payments
                .Where(p => p.Appointment.PatientId == patient.Id)
                .Include(p => p.Appointment)
                .ToListAsync();

            return View("PatientDetails");
        }
    }
}