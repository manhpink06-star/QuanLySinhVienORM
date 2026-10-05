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

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Lớp")]
        public string Lop { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }

        [Display(Name = "Điểm trung bình")]
        [Range(0, 10)]
        public double DiemTrungBinh { get; set; }
    }
}