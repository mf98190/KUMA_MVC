﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using KUMA_MVC.Models;
using KUMA_MVC.Services;
using KUMA_MVC.ViewModels;
using Dapper;
using Microsoft.AspNet.Identity;
using KUMA_MVC.Helpers;
using System.Data.Entity;
using KUMA_MVC.Repositories;

namespace KUMA_MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        protected KumaModel db;

        public HomeController()
        {
            db = new KumaModel();
        }

        [AllowAnonymous]
        [OutputCache(CacheProfile = "HomeIndex")]
        public ActionResult Index()   //主頁面
        {
            return View();
        }

        [AllowAnonymous]
        [OutputCache(CacheProfile = "ProductsPage")]
        public ActionResult ProductsPage(string Category, int page, string sortOrder) //商品頁面
        {
            var products = from p in db.Products select p;
            //分類
            switch (Category)
            {
                case "Bracelets":
                    products = products.Where(x => x.CategoryID == 1);
                    ViewBag.Title = "BRACELETS";
                    ViewBag.Category = "Bracelets";
                    break;
                case "Ear Rings":
                    products = products.Where(x => x.CategoryID == 2);
                    ViewBag.Title = "EAR RINGS";
                    ViewBag.Category = "Ear Rings";
                    break;
                case "Necklaces":
                    products = products.Where(x => x.CategoryID == 3);
                    ViewBag.Title = "NECKLACES";
                    ViewBag.Category = "Necklaces";
                    break;
                case "Rings":
                    products = products.Where(x => x.CategoryID == 4);
                    ViewBag.Title = "RINGS";
                    ViewBag.Category = "Rings";
                    break;
                default:
                    ViewBag.Title = "PRODUCTS";
                    ViewBag.Category = "All";
                    break;
            }
            //排序
            switch (sortOrder)
            {
                case "PriceHighToLow":
                    products = products.OrderByDescending(x => x.UnitPrice);
                    ViewBag.SortOrder = "PriceHighToLow";
                    break;
                case "PriceLowToHigh":
                    products = products.OrderBy(x => x.UnitPrice);
                    ViewBag.SortOrder = "PriceLowToHigh";
                    break;
                case "Name":
                    products = products.OrderBy(x => x.ProductName);
                    ViewBag.SortOrder = "Name";
                    break;
                case "Name_desc":
                    products = products.OrderByDescending(x => x.ProductName);
                    ViewBag.SortOrder = "Name_desc";
                    break;
                default:
                    ViewBag.SortOrder = "";
                    break;
            }

            ViewData.Model = products.ToList();

            ViewBag.Page = page;

            return View();
        }

        [AllowAnonymous]
        public ActionResult GetProducts(int page, List<Product> AllProducts)
        {
            //每八個一頁
            var ProductNumber = 8;
            //取得目前頁所需要顯示的物品
            var pList = AllProducts.Skip((page - 1) * ProductNumber).Take(ProductNumber).ToList();

            //建立全部商品
            ProductsService service = new ProductsService(db);
            var AllProductDetails = service.getPageOfProducts();
            var Model = new List<ProductPageViewModel>();

            for (var i = 0; i < pList.Count; i++)
            {
                //避免超出範圍，雖然不一定會用到
                if (i % 8 == 0 && i != 0) break;

                //取得此產品ID所有的PDID，並加進List<ProductPageViewModel> Model中
                var FilterList = AllProductDetails.Where(x => x.ProductID == pList[i].ProductID);
                foreach (var item in FilterList)
                {
                    Model.Add(item);
                }
            }
            ViewData.Model = Model;
            return PartialView("_ProductsPartial");
        }

        //---------------------單一商品頁面-----------------------
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ProductDetailPage(int pid)  //單一商品頁面 Get
        {
            ProductViewModelService service = new ProductViewModelService(db, pid);
            var sPVM = service.PVM;
            ViewData.Model = sPVM;
            ViewBag.ColorList = db.Colors.ToList();
            ViewBag.JsonPVM = Newtonsoft.Json.JsonConvert.SerializeObject(sPVM.ProductDetailViewModels);

            ViewData["ColorName"] = sPVM.ProductDetailViewModels[0].ColorName;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ProductDetailPage(int pid, string DropDownList_Color)  //單一商品頁面Post
        {
            ProductViewModelService service = new ProductViewModelService(db, pid);
            var sPVM = service.PVM;
            ViewData.Model = sPVM;
            ViewBag.ColorList = db.Colors.ToList();
            ViewBag.JsonPVM = Newtonsoft.Json.JsonConvert.SerializeObject(sPVM.ProductDetailViewModels);

            ViewData["ColorName"] = DropDownList_Color;
            return View("ProductDetailPage");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult BuyItNow(string pdid)       //立即購買按鈕
        {
            var connectionString = ConfigurationManager.ConnectionStrings["KumaConnection"].ConnectionString;
            string queryString = @"select 
                                 cat.CategoryName, 
                                 p.ProductID, 
                                 p.ProductName, 
                                 p.UnitPrice, 
                                 s.SizeContext, 
                                 c.ColorID, 
                                 c.ColorName, 
                                 pd.PDID 
                                 from Products p 
                                 inner join ProductDetails pd on p.ProductID = pd.ProductID 
                                 inner join Sizes s on pd.SizeID = s.SizeID 
                                 inner join Colors c on pd.ColorID = c.ColorID 
                                 inner join Categories cat on p.CategoryID = cat.CategoryID 
                                 where pd.PDID = " + "'" + pdid + "'";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                CartItemViewModel product = conn.QueryFirstOrDefault<CartItemViewModel>(queryString);
                string image = string.Empty;

                product.Quantity = 1;
                ViewData.Model = product;

                if (db.Images.Any(x => x.PDID == pdid))
                {
                    image = db.Images.First(x => x.PDID == pdid).ImgName;
                }
                //如果這個物品本身沒有圖片
                else
                {
                    var sameProductImage =      //找出此物品所屬的產品所有關聯的圖片
                        from p in db.Products
                        join pd in db.ProductDetails on p.ProductID equals pd.ProductID
                        join img in db.Images on pd.PDID equals img.PDID
                        where p.ProductID == product.ProductID && pd.ColorID == product.ColorID
                        select new
                        {
                            ImgName = img.ImgName
                        };
                    image = sameProductImage.First().ImgName;
                }

                Session["CartToHere"] = false;
                Session["BuyItNow"] = new BuyOneViewModel() { CartItem = product, Image = image };
                //這是傳給HttpGet喔！
                return RedirectToAction("Order_Customer", "Home"/*, new {product = product, image = image}*/);
            }
        }
        //-----------------------------------------------------------------
        //----------------------------下單-----------------------------
        [HttpGet]
        [Authorize]
        public ActionResult Order_Customer(/*CartItemViewModel product,string image*/)  //下單-客戶頁面(填入收件人)!沒有HEADER跟FOOTER
        {
            var Img = CartService.getEachProductImages(db);
            ViewBag.Img = Img;

            if (Session["Order_Session"] != null)
            {
                var OCVM = (OrderCustomerViewModel)Session["Order_Session"];
                ViewData["CustomerName"] = OCVM.CustomerName;
                ViewData["City"] = OCVM.City;
                ViewData["ZipCode"] = OCVM.ZipCode;
                ViewData["Address"] = OCVM.Address;
                ViewData["Phone"] = OCVM.Phone;
                ViewData["Email"] = OCVM.Email;
            }
            var Userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.FirstOrDefault(x => x.Id == Userid);
            ViewData["Email"] = user.Email;
            ViewData["UserName"] = user.Name;
            ViewBag.CartToHere = Session["CartToHere"];
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Order_Customer(OrderCustomerViewModel OCVM)
        {
            Session["Order_Session"] = OCVM;
            ViewData["CustomerName"] = OCVM.CustomerName;
            ViewData["City"] = OCVM.City;
            ViewData["ZipCode"] = OCVM.ZipCode;
            ViewData["Address"] = OCVM.Address;
            ViewData["Phone"] = OCVM.Phone;
            ViewData["Email"] = OCVM.Email;
            ViewBag.CartToHere = Session["CartToHere"];
            List<Shipper> shippers = db.Shippers.ToList();
            ViewBag.Shippers = shippers;
            return View("Order_Ship");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Order_Ship()  //下單-運送頁面!沒有HEADER跟FOOTER
        {
            var OCVM = (OrderCustomerViewModel)Session["Order_Session"];
            ViewData["Email"] = OCVM.Email;
            ViewData["Address"] = OCVM.Address;
            ViewBag.CartToHere = Session["CartToHere"];
            ViewData.Model = OCVM;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Order_Ship([Bind(Include = "ShipperID")]int ShipperID)  //下單-運送頁面!沒有HEADER跟FOOTER
        {
            var OCVM = (OrderCustomerViewModel)Session["Order_Session"];
            Shipper shipper = db.Shippers.FirstOrDefault(x => x.ShipperID == ShipperID);
            OCVM.ShipperID = ShipperID;
            OCVM.ShippName = shipper.ShippName;
            OCVM.Fare = shipper.Fare;
            ViewData["CustomerName"] = OCVM.CustomerName;
            ViewData["City"] = OCVM.City;
            ViewData["ZipCode"] = OCVM.ZipCode;
            ViewData["Address"] = OCVM.Address;
            ViewData["Phone"] = OCVM.Phone;
            ViewData["Email"] = OCVM.Email;
            ViewData["ShippName"] = OCVM.ShippName;
            ViewData["Fare"] = OCVM.Fare;
            ViewBag.CartToHere = Session["CartToHere"];
            ViewData.Model = OCVM;

            return View("Order_Pay");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Order_Pay()  //下單-付費頁面!沒有HEADER跟FOOTER
        {
            var OCVM = (OrderCustomerViewModel)Session["Order_Session"];
            ViewData["Email"] = OCVM.Email;
            ViewData["Address"] = OCVM.Address;
            ViewData.Model = OCVM;
            ViewBag.CartToHere = Session["CartToHere"];
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Order_Pay(OrderCustomerViewModel Bill_OCVM, bool Order_Pay_Radio)  //下單-付費頁面!沒有HEADER跟FOOTER
        {
            var OCVM = (OrderCustomerViewModel)Session["Order_Session"];
            ViewData["CustomerName"] = OCVM.CustomerName;
            ViewData["City"] = OCVM.City;
            ViewData["ZipCode"] = OCVM.ZipCode;
            ViewData["Address"] = OCVM.Address;
            ViewData["Phone"] = OCVM.Phone;
            ViewData["Email"] = OCVM.Email;

            if (Order_Pay_Radio)
            {
                Session["Bill_Order_Seccion"] = Bill_OCVM;
                ViewData["Bill_CustomerName"] = Bill_OCVM.Bill_CustomerName;
                ViewData["Bill_City"] = Bill_OCVM.Bill_City;
                ViewData["Bill_ZipCode"] = Bill_OCVM.Bill_ZipCode;
                ViewData["Bill_Address"] = Bill_OCVM.Bill_Address;
                ViewData["Bill_Phone"] = Bill_OCVM.Bill_Phone;
            }

            //SQL

            var REPO_O = new Repositories.KumaRepository<Order>(db);
            var REPO_OD = new Repositories.KumaRepository<OrderDetail>(db);
            Random rnd = new Random();
            List<string> name = new List<string>()
            {
                "風志淇", "風志聞", "風志來", "風志可", "風志成", "風志偉"
            };
            List<string> country = new List<string>()
            {
                "Taiwan 台灣", "Thailand 泰國", "Sweden 瑞典", "Poland 波蘭", "Russia 俄羅斯", "Malaysia 馬來西亞", "Japan 日本", "India 印度", "United States 美國"
            };
            List<string> payment = new List<string>()
            {
                "銀行轉帳", "信用卡/VISA金融卡", "貨到付款"
            };
            List<string> zipCode = new List<string>()
            {
                "300", "226", "267", "413", "362", "649", "901"
            };
            List<string> city = new List<string>()
            {
                "Keelung", "Taipei", "Hsinchu", "Taichung", "Tainan", "Kaohsiung", "Yilan", "Taoyuan", "Miaoli", "Changhua", "Nantou"
            };
            List<string> address = new List<string>()
            {
                "No. 1, Sec. 4, Roosevelt Rd., Da’an Dist.", "No. 2, Ln. 456, Sec. 1, Xinsheng S. Rd., Da’an Dist.", "No. 456, Sec. 2, Heping E. Rd., Da’an Dist.", "No. 85, Sec. 1, Jianguo S. Rd., Da’an Dist.",
                "No. 85, Sec. 1, Jianguo S. Rd., Da’an Dist.", "No. 67, Sec. 2, Yangde Blvd., Shilin Dist.", "No. 865, Yishou St., Wenshan Dist."
            };
            List<string> phone = new List<string>()
            {
                "+639051234567", "+817012345678", "+85261234567", "+12042345678", "+5491123456789", "+358412345678"
            };
            List<decimal> fare = new List<decimal>()
            {
                60,60,60,80,90
            };
            var O = new Order();

            // 存到Order資料庫
            O.RecipientName = name[rnd.Next(name.Count)];
            O.RecipientCity = city[rnd.Next(city.Count)];
            O.RecipientZipCod = zipCode[rnd.Next(zipCode.Count)];
            O.RecipientAddressee = address[rnd.Next(address.Count)];
            O.RecipientPhone = phone[rnd.Next(phone.Count)];
            int n = rnd.Next(fare.Count); 
            O.ShippingID = n+1;
            O.Fare_now = fare[n];
            O.RecipientCountry = country[rnd.Next(country.Count)];

            var Userid = User.Identity.GetUserId();
            O.UserID = Userid;
            //因為目前只有轉帳功能
            O.Payment = payment[rnd.Next(payment.Count)];
            if (Session["Remark"] == null)
            {
                O.Remaeks = null;
            }
            else
            {
                O.Remaeks = Session["Remark"].ToString();
            }
            O.OrderDate = DateTime.Now;
            O.StatusID = 7;
            O.TotalPrice = 0;
            //O.ShipDate = DateTime.Now.AddHours(1);

            REPO_O.Create(O);
            try
            {
            db.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var OrderList = db.Orders.ToList();
            var OrderID = OrderList.Last().OrderID;

            var REPO_PD = new KumaRepository<ProductDetail>(db);
            var PDs = db.ProductDetails.ToList();
            if ((bool)Session["CartToHere"])
            {
                var currentCart = CartService.GetCurrentCart();
                foreach (var item in currentCart)
                {
                    //新增訂單
                    var OD = new OrderDetail();
                    OD.PDID = item.PDID;
                    OD.OrderID = OrderID;
                    OD.Price = item.Amount;
                    OD.Quantity = item.Quantity;
                    REPO_OD.Create(OD);

                    //修正物品庫存量
                    var pd = PDs.FirstOrDefault(x => x.PDID == OD.PDID);
                    pd.Stock -= OD.Quantity;
                    REPO_PD.Update(pd);
                }
                //加入Order總金額
                try
                {
                    O = db.Orders.ToList().LastOrDefault(x => x.OrderID == OrderID);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                O.TotalPrice = currentCart.TotalAmount + OCVM.Fare;
                REPO_O.Update(O);
            }
            else // BuyItNow
            {
                //新增訂單
                var OD = new OrderDetail();
                var BuyItNow = (BuyOneViewModel)Session["BuyItNow"];
                OD.PDID = BuyItNow.CartItem.PDID;
                OD.OrderID = OrderID;
                OD.Price = BuyItNow.CartItem.UnitPrice;
                OD.Quantity = BuyItNow.CartItem.Quantity;
                REPO_OD.Create(OD);
                //加入Order總金額
                try
                {
                    O = db.Orders.ToList().LastOrDefault(x => x.OrderID == OrderID);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                O.TotalPrice = BuyItNow.CartItem.Amount + OCVM.Fare;
                REPO_O.Update(O);

                //修正物品庫存量
                var pd = PDs.FirstOrDefault(x => x.PDID == OD.PDID);
                pd.Stock -= OD.Quantity;
                REPO_PD.Update(pd);
            }
            db.SaveChanges();

            //初始化
            Session["Remark"] = null;
            ViewData.Model = CartService.GetCurrentCart();
            ViewBag.Img = CartService.getEachProductImages(db);
            //購買完成，清除購物車
            CartService.ClearCart();
            return View("Order_Check");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Order_Check()  //下單-確認頁面!沒有HEADER跟FOOTER
        {
            var OCVM = (OrderCustomerViewModel)Session["Order_Session"];
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Order_Check(bool Order_Pay_Radio)  //下單-確認頁面!沒有HEADER跟FOOTER
        {
            var OCVM = (OrderCustomerViewModel)Session["Order_Session"];
            ViewData["CustomerName"] = OCVM.CustomerName;
            ViewData["City"] = OCVM.City;
            ViewData["ZipCode"] = OCVM.ZipCode;
            ViewData["Address"] = OCVM.Address;
            ViewData["Phone"] = OCVM.Phone;
            ViewData["Email"] = OCVM.Email;

            if (Order_Pay_Radio)
            {

                var Bill_OCVM = (OrderCustomerViewModel)Session["Bill_Order_Seccion"];
                ViewData["Bill_CustomerName"] = Bill_OCVM.Bill_CustomerName;
                ViewData["Bill_City"] = Bill_OCVM.Bill_City;
                ViewData["Bill_ZipCode"] = Bill_OCVM.Bill_ZipCode;
                ViewData["Bill_Address"] = Bill_OCVM.Bill_Address;
                ViewData["Bill_Phone"] = Bill_OCVM.Bill_Phone;

            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult getOrderPartial(CartModel currentCart, string[] images)   //導向Partial
        {
            var Img = CartService.getEachProductImages(db);
            ViewBag.Img = Img;
            if (currentCart.Count != 0)
            {
                ViewBag.Img = images;
                ViewData.Model = currentCart;
            }
            return PartialView("_OrderPartial");
        }

        [HttpGet]
        [Authorize]
        //[AllowAnonymous]
        public ActionResult AccountPage()   //帳戶主頁面
        {
            AccountPageViewModel model = new AccountPageViewModel();
            var userID = User.Identity.GetUserId();
            model.User = db.AspNetUsers.FirstOrDefault(x => x.Id == userID);
            if (db.Orders.Any(x => x.UserID == userID))
            {
                List<Order> orders = db.Orders.Where(x => x.UserID == userID).OrderByDescending(x => x.OrderID).ToList();
                model.Orders = orders;
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                List<Image> images = new List<Image>();
                int endIndext = 0;
                string PDIDtoFindImg = string.Empty;
                foreach (var item in orders)
                {
                    var thisOrderDetails = db.OrderDetails.Where(x => x.OrderID == item.OrderID).ToList();
                    foreach (var sonitem in thisOrderDetails)
                    {
                        orderDetails.Add(sonitem);
                        endIndext = sonitem.PDID.IndexOf("-");
                        PDIDtoFindImg = sonitem.PDID.Substring(0, endIndext) + "-1";
                        if (!images.Any(x => x.PDID == PDIDtoFindImg))
                        {
                            images.Add(db.Images.First(x => x.PDID == PDIDtoFindImg));
                        }
                    }
                }
                model.OrderDetails = orderDetails;
                model.images = images;
            }
            ViewBag.City = new SelectList(ConstantData.Citys, model.User.City);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
        public ActionResult AccountPage([Bind(Include = "Name, Email, PhoneNumber, City, Address, ZipCode")] UserViewModel model)   //帳戶主頁面
        {
            if (ModelState.IsValid)
            {
                var Userid = User.Identity.GetUserId();
                var user = db.AspNetUsers.FirstOrDefault(x => x.Id == Userid);

                user.Name = model.Name;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.City = model.City;
                user.Address = model.Address;
                user.ZipCode = model.ZipCode;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("AccountPage", "Home");

        }
    }
}