using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital_Project.Data;
using Hospital_Project.Models;

namespace Hospital_Project.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Doctor)
                .ToListAsync();
            return View(payments);
        }

        // GET: Payments/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => string.IsNullOrEmpty(a.AppointmentStatus) || a.AppointmentStatus != "دفع مكتمل")
                .ToListAsync();
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: Payments/Create
        public async Task<IActionResult> Create(int? appointmentId = null)
        {
            var appointmentsQuery = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor);

            if (appointmentId.HasValue)
            {
                // لو جا من صفحة المواعيد، نعرض الموعد ده فقط
                ViewBag.SelectedAppointmentId = appointmentId.Value;
                ViewBag.Appointments = await appointmentsQuery
                    .Where(a => a.Id == appointmentId.Value && a.AppointmentStatus != "دفع مكتمل")
                    .ToListAsync();
            }
            else
            {
                // لو دخل من صفحة المدفوعات مباشرةً
                ViewBag.Appointments = await appointmentsQuery
                    .Where(a => a.AppointmentStatus != "دفع مكتمل")
                    .ToListAsync();
            }

            if (!ViewBag.Appointments.Any())
            {
                TempData["Error"] = "لا يوجد مواعيد صالحة للدفع.";
                return RedirectToAction("Index", "Payments");
            }

            return View();
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _context.Payments
                .Include(p => p.Appointment)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null) return NotFound();
            ViewBag.Appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payments payment)
        {
            if (id != payment.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "تم تحديث الدفع بنجاح!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            ViewBag.Appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _context.Payments
                .Include(p => p.Appointment)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                // إلغاء حالة الدفع من الموعد
                var appointment = await _context.Appointments.FindAsync(payment.AppointmentId);
                if (appointment != null)
                {
                    appointment.AppointmentStatus = "معلق";
                    _context.Update(appointment);
                }
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف الدفع بنجاح!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id) => _context.Payments.Any(e => e.Id == id);
    }
}