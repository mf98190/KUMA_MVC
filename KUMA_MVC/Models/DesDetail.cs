namespace KUMA_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DesDetail
    {
        [Key]
        public int DDID { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string Detail { get; set; }

        public virtual Product Product { get; set; }
    }
}
