namespace KUMA_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string PDID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? Discount { get; set; }

        public virtual Order Order { get; set; }

        public virtual ProductDetail ProductDetail { get; set; }
    }
}
