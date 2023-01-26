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

        public string? ParentKey { get; set; }

        [Required(ErrorMessage = "이름을 입력하세요.")]
        [MaxLength(255)]
        public string? Name { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public bool? IsPinned { get; set; } = false;

        public int ReadCount { get; set; }


        [Display(Name = "파일")]
        public string? FileName { get; set; }


        [Display(Name = "파일크기")]
        public int FileSize { get; set; }


        [Display(Name = "다운수")]
        public int DownCount { get; set; }

        /// <summary>
        /// 참조(부모글)
        /// </summary>
        public int Ref { get; set; }

        /// <summary>
        /// 답변깊이(레벨)
        /// </summary>
        public int? Step { get; set; }

        /// <summary>
        /// 답변순서
        /// </summary>
        public int? RefOrder { get; set; }

    }
}
