﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KUMA_MVC.Models;

namespace KUMA_MVC.ViewModels
{
    public class ProductDetailViewModel
    {
        public int ProductID { get; set; }
        public string PDID { get; set; }
        public int Stock { get; set; }
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public int SizeID { get; set; }
        public string SizeTitle { get; set; }
        public string SizeContext { get; set; }
    }
}