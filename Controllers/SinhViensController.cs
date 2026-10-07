using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVienORM.Data;
using QuanLySinhVienORM.Models;

namespace QuanLySinhVienORM.Controllers
{
    public class SinhViensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SinhViensController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // =====================================================
        // DANH SÁCH SINH VIÊN
        // =====================================================
        public async Task<IActionResult> Index()
        {
            return View(await _context.SinhViens.ToListAsync());
        }

        // =====================================================
        // CHI TIẾT SINH VIÊN
        // =====================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens
                .FirstOrDefaultAsync(s => s.MaSV == id);

            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // =====================================================
        // THÊM - GET
        // =====================================================
        public IActionResult Create()
        {
            return View();
        }

        // =====================================================
        // THÊM - POST + UPLOAD ẢNH
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            SinhVien sinhVien,
            IFormFile? imageFile)
        {
            // Kiểm tra file nếu người dùng chọn ảnh
            if (imageFile != null && imageFile.Length > 0)
            {
                string extension = Path
                    .GetExtension(imageFile.FileName)
                    .ToLowerInvariant();

                string[] allowedExtensions =
                {
                    ".jpg",
                    ".jpeg",
                    ".png"
                };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        "HinhAnh",
                        "Chỉ được upload file JPG, JPEG hoặc PNG."
                    );
                }
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string extension = Path
                        .GetExtension(imageFile.FileName)
                        .ToLowerInvariant();

                    // Tạo tên file mới để tránh trùng
                    string fileName =
                        Guid.NewGuid().ToString() + extension;

                    // wwwroot/images/students
                    string folderPath = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "images",
                        "students"
                    );

                    // Tự tạo thư mục nếu chưa có
                    Directory.CreateDirectory(folderPath);

                    string filePath = Path.Combine(
                        folderPath,
                        fileName
                    );

                    // Lưu ảnh
                    using (var stream =
                           new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Chỉ lưu tên ảnh vào database
                    sinhVien.HinhAnh = fileName;
                }

                _context.SinhViens.Add(sinhVien);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(sinhVien);
        }

        // =====================================================
        // SỬA - GET
        // =====================================================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien =
                await _context.SinhViens.FindAsync(id);

            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // =====================================================
        // SỬA - POST
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            SinhVien sinhVien,
            IFormFile? imageFile)
        {
            if (id != sinhVien.MaSV)
            {
                return NotFound();
            }

            // Lấy thông tin cũ nhưng không Tracking
            var sinhVienCu = await _context.SinhViens
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.MaSV == id);

            if (sinhVienCu == null)
            {
                return NotFound();
            }

            // Kiểm tra ảnh mới
            if (imageFile != null && imageFile.Length > 0)
            {
                string extension = Path
                    .GetExtension(imageFile.FileName)
                    .ToLowerInvariant();

                string[] allowedExtensions =
                {
                    ".jpg",
                    ".jpeg",
                    ".png"
                };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        "HinhAnh",
                        "Chỉ được upload file JPG, JPEG hoặc PNG."
                    );
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Nếu có chọn ảnh mới
                    if (imageFile != null &&
                        imageFile.Length > 0)
                    {
                        string extension = Path
                            .GetExtension(imageFile.FileName)
                            .ToLowerInvariant();

                        string fileName =
                            Guid.NewGuid().ToString()
                            + extension;

                        string folderPath = Path.Combine(
                            _webHostEnvironment.WebRootPath,
                            "images",
                            "students"
                        );

                        Directory.CreateDirectory(folderPath);

                        string filePath =
                            Path.Combine(
                                folderPath,
                                fileName
                            );

                        // Lưu ảnh mới
                        using (var stream =
                               new FileStream(
                                   filePath,
                                   FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Xóa ảnh cũ
                        if (!string.IsNullOrEmpty(
                                sinhVienCu.HinhAnh))
                        {
                            string oldImagePath =
                                Path.Combine(
                                    folderPath,
                                    sinhVienCu.HinhAnh
                                );

                            if (System.IO.File.Exists(
                                    oldImagePath))
                            {
                                System.IO.File.Delete(
                                    oldImagePath
                                );
                            }
                        }

                        sinhVien.HinhAnh = fileName;
                    }
                    else
                    {
                        // Không chọn ảnh mới thì giữ ảnh cũ
                        sinhVien.HinhAnh =
                            sinhVienCu.HinhAnh;
                    }

                    _context.Update(sinhVien);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinhVienExists(sinhVien.MaSV))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            // Validation lỗi thì vẫn giữ ảnh cũ
            sinhVien.HinhAnh = sinhVienCu.HinhAnh;

            return View(sinhVien);
        }

        // =====================================================
        // XÓA - GET
        // =====================================================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens
                .FirstOrDefaultAsync(s => s.MaSV == id);

            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // =====================================================
        // XÓA - POST
        // =====================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sinhVien =
                await _context.SinhViens.FindAsync(id);

            if (sinhVien != null)
            {
                // Xóa file ảnh nếu có
                if (!string.IsNullOrEmpty(sinhVien.HinhAnh))
                {
                    string imagePath = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "images",
                        "students",
                        sinhVien.HinhAnh
                    );

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Xóa dữ liệu database
                _context.SinhViens.Remove(sinhVien);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SinhVienExists(int id)
        {
            return _context.SinhViens
                .Any(s => s.MaSV == id);
        }
    }
}