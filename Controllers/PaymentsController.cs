

namespace Hospital_Project.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Payments
                .Include(p => p.Appointment)
                .ThenInclude(a => a.Patient)
                .ToListAsync());
        }

        public async Task<IActionResult> Create(int? appointmentId = null)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Where(a => a.AppointmentStatus != "دفع مكتمل");

            if (appointmentId.HasValue)
            {
                ViewBag.SelectedAppointmentId = appointmentId.Value;
                ViewBag.Appointments = await query.Where(a => a.Id == appointmentId).ToListAsync();
            }
            else
            {
                ViewBag.Appointments = await query.ToListAsync();
            }

            var list = ViewBag.Appointments as List<Appointment>;
            if (list == null || !list.Any())
            {
                TempData["Error"] = "لا يوجد مواعيد صالحة للدفع.";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payments payment)
        {
            if (ModelState.IsValid)
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                var appt = await _context.Appointments.FindAsync(payment.AppointmentId);
                if (appt != null)
                {
                    appt.AppointmentStatus = "دفع مكتمل";
                    _context.Update(appt);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Appointments = await _context.Appointments
                .Where(a => a.AppointmentStatus != "دفع مكتمل")
                .ToListAsync();
            return View(payment);
        }

    

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
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