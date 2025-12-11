using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital_Project.Data;
using Hospital_Project.Models;

namespace Hospital_Project.Controllers
{
    public class MedicalRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedicalRecords
        public async Task<IActionResult> Index()
        {
            var records = await _context.MedicalRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Include(r => r.Appointment)
                .ToListAsync();
            return View(records);
        }

        // GET: MedicalRecords/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Doctors = await _context.Doctors.ToListAsync();
            ViewBag.Appointments = await _context.Appointments.ToListAsync();
            return View();
        }

        // POST: MedicalRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalRecord record)
        {
            if (ModelState.IsValid)
            {
                _context.MedicalRecords.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Doctors = await _context.Doctors.ToListAsync();
            ViewBag.Appointments = await _context.Appointments.ToListAsync();
            return View(record);
        }
        // GET: MedicalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var medicalRecord = await _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Include(m => m.Appointment)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalRecord == null) return NotFound();

            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Doctors = await _context.Doctors.ToListAsync();
            ViewBag.Appointments = await _context.Appointments.ToListAsync();
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicalRecord medicalRecord)
        {
            if (id != medicalRecord.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalRecordExists(medicalRecord.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Doctors = await _context.Doctors.ToListAsync();
            ViewBag.Appointments = await _context.Appointments.ToListAsync();
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var medicalRecord = await _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Include(m => m.Appointment)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalRecord == null) return NotFound();
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord != null)
                _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalRecordExists(int id) => _context.MedicalRecords.Any(e => e.Id == id);
    }
}