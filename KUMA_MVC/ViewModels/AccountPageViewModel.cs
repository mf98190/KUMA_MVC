using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KUMA_MVC.Models;

namespace KUMA_MVC.ViewModels
{
    public class AccountPageViewModel
    {
        public AspNetUser User { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Image> images { get; set; }
    }
}