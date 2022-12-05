namespace ArticleApp.Models.Common
{
    /// <summary>
    /// 레코드에 대한 상태 추적을 위한 4개의 속성 제공
    /// </summary>
    public class AuditableBase
    {
        /// <summary>
        /// 등록자
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 등록일
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 수정자
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// 수정일
        /// </summary>
        public DateTime? Modified { get; set; }
    }
}