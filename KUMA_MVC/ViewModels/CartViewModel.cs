using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KUMA_MVC.Models;

namespace KUMA_MVC.ViewModels
{
    public class CartViewModel
    {
        public CartModel Cart { get; set; }
        public string[] Images { get; set; }
    }
}