namespace KUMA_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Support
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SupportID { get; set; }

        [Required]
        [StringLength(128)]
        public string UserID { get; set; }

        public int SupportCategoryID { get; set; }

        [Required]
        [StringLength(128)]
        [Column(TypeName = "nvarchar")]
        public string SupportTitle { get; set; }

        [Required]
        public string SupportContent { get; set; }

        public int StatusID { get; set; }

        public DateTime SupportTime { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Status Status { get; set; }

        public virtual SupportCategory SupportCategory { get; set; }
    }
}
