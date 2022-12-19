using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoticeApp.Models
{
    [Table(name: "Notices")]
    public class Notice : AuditableBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "이름을 입력하세요.")]
        [MaxLength(255)]
        public string? Name { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public bool? IsPinned { get; set; } = false;
    }
}
