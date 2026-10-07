using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVienORM.Models
{
    public class SinhVien
    {
        [Key]
        public int MaSV { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập lớp")]
        [Display(Name = "Lớp")]
        public string Lop { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm trung bình phải từ 0 đến 10")]
        [Display(Name = "Điểm trung bình")]
        public double DiemTrungBinh { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? HinhAnh { get; set; }
    }
}