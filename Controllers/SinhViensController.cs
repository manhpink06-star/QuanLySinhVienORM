
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVienORM.Models;
using QuanLySinhVienORM.Data;

public class SinhViensController : Controller
{
    private readonly ApplicationDbContext _context;

    public SinhViensController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: SINHVIENS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.SinhViens.ToListAsync());
    }

    // GET: SINHVIENS/Details/5
    public async Task<IActionResult> Details(int? masv)
    {
        if (masv == null)
        {
            return NotFound();
        }

        var sinhvien = await _context.SinhViens
            .FirstOrDefaultAsync(m => m.MaSV == masv);
        if (sinhvien == null)
        {
            return NotFound();
        }

        return View(sinhvien);
    }

    // GET: SINHVIENS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SINHVIENS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaSV,HoTen,NgaySinh,GioiTinh,Lop,Email,DiemTrungBinh")] SinhVien sinhvien)
    {
        if (ModelState.IsValid)
        {
            _context.Add(sinhvien);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(sinhvien);
    }

    // GET: SINHVIENS/Edit/5
    public async Task<IActionResult> Edit(int? masv)
    {
        if (masv == null)
        {
            return NotFound();
        }

        var sinhvien = await _context.SinhViens.FindAsync(masv);
        if (sinhvien == null)
        {
            return NotFound();
        }
        return View(sinhvien);
    }

    // POST: SINHVIENS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? masv, [Bind("MaSV,HoTen,NgaySinh,GioiTinh,Lop,Email,DiemTrungBinh")] SinhVien sinhvien)
    {
        if (masv != sinhvien.MaSV)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(sinhvien);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SinhVienExists(sinhvien.MaSV))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(sinhvien);
    }

    // GET: SINHVIENS/Delete/5
    public async Task<IActionResult> Delete(int? masv)
    {
        if (masv == null)
        {
            return NotFound();
        }

        var sinhvien = await _context.SinhViens
            .FirstOrDefaultAsync(m => m.MaSV == masv);
        if (sinhvien == null)
        {
            return NotFound();
        }

        return View(sinhvien);
    }

    // POST: SINHVIENS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? masv)
    {
        var sinhvien = await _context.SinhViens.FindAsync(masv);
        if (sinhvien != null)
        {
            _context.SinhViens.Remove(sinhvien);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SinhVienExists(int? masv)
    {
        return _context.SinhViens.Any(e => e.MaSV == masv);
    }
}
