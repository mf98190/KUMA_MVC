using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using KUMA_MVC.Models;
using KUMA_MVC.ViewModels;
using Dapper;

namespace KUMA_MVC.Services
{
    public class ProductsService
    {
        private KumaModel db;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["KumaConnection"].ConnectionString;
        public ProductsService(KumaModel db)
        {
            this.db = db;
        }
        public IEnumerable<ProductPageViewModel> getPageOfProducts()
        {
            string queryString = "select " +
                                 "cat.CategoryName, " +
                                 "p.ProductID, " +
                                 "p.UnitPrice, " +
                                 "p.ProductName, " +
                                 "pd.PDID, " +
                                 "pd.ColorID, " +
                                 "c.ColorName, " +
                                 "i.ImgName " +
                                 "from Products p " +
                                 "inner join Categories cat on p.CategoryID = cat.CategoryID " +
                                 "inner join ProductDetails pd on p.ProductID = pd.ProductID " +
                                 "left outer join Images i on i.PDID = pd.PDID " +
                                 "inner join Colors c on pd.ColorID = c.ColorID " +
                                 "order by p.ProductID,pd.PDID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var pList = conn.Query<ProductPageViewModel>(queryString);
                return pList;
            }
        }
    }
}