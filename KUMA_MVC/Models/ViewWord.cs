namespace KUMA_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ViewWord
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string WordName { get; set; }

        [Required]
        public string WordContent { get; set; }
    }
}
