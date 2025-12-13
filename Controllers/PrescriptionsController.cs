using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital_Project.Data;
using Hospital_Project.Models;

namespace Hospital_Project.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prescriptions
        public async Task<IActionResult> Index()
        {
            var prescriptions = await _context.Prescriptions
                .Include(p => p.MedicalRecord)
                    .ThenInclude(mr => mr.Patient)
                .Include(p => p.MedicalRecord)
                    .ThenInclude(mr => mr.Doctor)
                .ToListAsync();
            return View(prescriptions);
        }

        public async Task<IActionResult> Create(int? medicalRecordId)
        {
            if (medicalRecordId == null)
            {
                ViewBag.MedicalRecords = await _context.MedicalRecords
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .ToListAsync();
                return View();
            }

            var record = await _context.MedicalRecords.FindAsync(medicalRecordId);
            if (record == null)
            {
                return NotFound();
            }

            ViewBag.SelectedMedicalRecord = record;
            return View(new Prescription { MedicalRecordId = record.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MedicalRecords = await _context.MedicalRecords
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .ToListAsync();
            return View(prescription);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var prescription = await _context.Prescriptions
                .Include(p => p.MedicalRecord)
                .ThenInclude(mr => mr.Patient)
                .Include(p => p.MedicalRecord)
                .ThenInclude(mr => mr.Doctor)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prescription == null) return NotFound();
            return View(prescription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prescription prescription)
        {
            if (id != prescription.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionExists(prescription.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(prescription);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var prescription = await _context.Prescriptions
                .Include(p => p.MedicalRecord)
                .ThenInclude(mr => mr.Patient)
                .Include(p => p.MedicalRecord)
                .ThenInclude(mr => mr.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prescription == null) return NotFound();
            return View(prescription);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription != null)
                _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionExists(int id) => _context.Prescriptions.Any(e => e.Id == id);
    }
}