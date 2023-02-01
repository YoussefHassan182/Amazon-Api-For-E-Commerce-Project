using ITI.ElectroDev.Models;
using ITI.ElectroDev.Presentation;
using ITI.Library.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AmazonApi.Controllers
{
    [ApiController]
   // [Route("[controller]")]
    [Route("[controller]/[action]")]
    public class ProductController : Controller
    {
        private Context db;
        public ProductController(Context _db)
        {
            db = _db;
        }

        [HttpGet]
        public ResultViewModel Get()
        {
            var products = db.Product.ToList();
           
            return new ResultViewModel()
            {
                Success = true,
                Message = "Products List",
                Data = new { 
                    products=products
                }
            };

        }
        [HttpGet("{id}")]
        public ResultViewModel Get(int id)
        {
            var products = db.Product.FirstOrDefault(i => i.Id == id);
            var brandName = db.Product.Where(i => i.Name == products.Name).Select(c => c.Brand.Name).FirstOrDefault();
            var catName = db.Product.Where(i => i.Name == products.Name).Select(c => c.Brand.Category.Name).FirstOrDefault();

            return new ResultViewModel()
            {
                Success = true,
                Message = "",
                Data = new {
                    categoryName = catName,
                    brandName = brandName,
                    Product = products
                
                }
            };


        }

        [HttpGet]
        public ResultViewModel getProductOffer()
        {
            List<Product> itemList = new List<Product>();

            var q = db.Product.Where(p => p.Discount != 0);
            foreach (var item in q)
            {
                itemList.Add(item);
            }
            return new ResultViewModel()
            {
                Success = true,
                Message = "Products List",
                Data = new
                {
                    productsOffer = itemList
                }
            };

        }
        [HttpGet]
        public ResultViewModel getProductBestSeller()
        {
            List<Product> itemList = new List<Product>();


            //var q = db.Product.Where(p => p.Quantity <= num);
            //  itemList = db.Product.OrderByDescending(x => x.OrderItems.Select(i=>i.ProductId).Count().Take(1).ToList();
            //foreach (var item in q)
            //{
            //    itemList.Add(item);
            //}
            itemList = db.Product.OrderByDescending(info => info.OrderItems.Select(i => new { i.ProductId, i.Quantity }).Sum(i => i.Quantity)).Take(10).ToList();
            return new ResultViewModel()
            {
                Success = true,
                Message = "Products List",
                Data = new
                {
                    productsOffer = itemList
                }
            };

        }

        [HttpGet]
        public ResultViewModel getProductMostPopular()
        {
            List<Product> itemList = new List<Product>();
            itemList = db.Product.OrderByDescending(x => x.OrderItems.Select(i=>i.ProductId).Count()).Take(10).ToList();
            return new ResultViewModel()
            {
                Success = true,
                Message = "Products List",
                Data = new
                {
                    productsOffer = itemList
                }
            };

        }

        [HttpGet]
        public ResultViewModel Search(string term)
        {
            var resulte = db.Product.Where(p => p.Name.Contains(term) || p.Description.Contains(term)).ToList();

            return new ResultViewModel()
            {
                Success = true,
                Message = "Products List",
                Data = new
                {
                    FilterdProducts = resulte,
                }
            };
        }

    }
}
