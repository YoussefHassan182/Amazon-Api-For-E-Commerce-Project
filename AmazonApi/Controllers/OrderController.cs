using AmazonApi.Models;
using Castle.Core.Resource;
using ITI.ElectroDev.Models;
using ITI.Library.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;

namespace AmazonApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private Context db;
        public OrderController(Context _db)
        {
            db = _db;
        }


        [HttpPost]
        public ResultViewModel Add(OrderCreateModel obj)
        {

            ResultViewModel myModel = new ResultViewModel();
            User user = db.Users.FirstOrDefault(i => i.Id == obj.UserId);
            if (ModelState.IsValid == false)
            {
                myModel.Success = false;
                myModel.Data =
                    ModelState.Values.SelectMany
                            (i => i.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                var product1 = db.Product.FirstOrDefault(i => i.Id == obj.orderItems.ProductId);
                var order = new OrderDetails
                {
                    CreatedAt = DateTime.Now,
                    UserId = user.Id,
                    PaymentMethod = obj.PaymentMethod,
                    Address = obj.Address,
                    TotalPrice = obj.TotalPrice,
                    Street = obj.Street,
                    Status = "Delivered"
                    
                };

                db.OrderDetails.Add(order);
                db.SaveChanges();
                var order_Item = new OrderItems
                {
                    OrderId = order.Id,
                    ProductId = obj.orderItems.ProductId,
                    Quantity = obj.orderItems.Quantity,
                    Price = obj.orderItems.Price
                };
                db.OrderItems.Add(order_Item);
                product1.Quantity = product1.Quantity - order_Item.Quantity;
                db.SaveChanges();
                myModel.Success = true;
                myModel.Message = "successful Order";
                myModel.Data = null;

            }
            return myModel;
        }


        //[HttpPost]        

        //public async Task<ResultViewModel> CreateNew(OrderCreateModel obj)
        //{

        //    ResultViewModel resultViewModel = new ResultViewModel();
        //    User user = db.Users.FirstOrDefault(i => i.Id == obj.UserId);
        //    if (user == null)
        //    {
        //        resultViewModel.Success = false;
        //        resultViewModel.Data = null;
        //        resultViewModel.Message = "User Not Found";
        //        return resultViewModel;
        //    }

        //    OrderDetails order = new OrderDetails()
        //    {
        //        CreatedAt = DateTime.Now,
        //        UserId = user.Id,
        //        PaymentMethod = obj.PaymentMethod,
        //        Address = obj.Address ,
        //        Street = obj.Street ,
        //        Status = "Delivered"
        //    };


        //    var _OrderItems = new OrderItems();

        //    var product = obj.orderItems;

        //        var product1 = db.Product.FirstOrDefault(i => i.Id == product.ProductId);
        //    if (product1 == null) { }

        //    else
        //    {

        //        OrderItems orderItems = new OrderItems();
        //        if (product1.Discount == 0)
        //        {
        //            orderItems.Quantity = product.Quantity;
        //            orderItems.OrderId = order.Id;
        //            orderItems.Price = product.Quantity * product1.Price;
        //            orderItems.ProductId = product.ProductId;
        //        }
        //        else
        //        {
        //            orderItems.Quantity = product.Quantity;
        //            orderItems.OrderId = order.Id;
        //            orderItems.Price = product.Quantity * ((product1.Price * product1.Discount) / 100);
        //            orderItems.ProductId = product.ProductId;
        //        }
        //        order.TotalPrice = order.TotalPrice + orderItems.Price;
        //            //reduce Quantity Product
        //            product1.Quantity = product1.Quantity - orderItems.Quantity;
        //            _OrderItems =orderItems;





        //    }

        //    db.OrderDetails.Add(order);
        //    db.SaveChanges();

        //    db.OrderItems.Add(_OrderItems);
        //    db.SaveChanges();


        //    resultViewModel.Success = true;
        //    //resultViewModel.Data = order;
        //    resultViewModel.Data = null;
        //    resultViewModel.Message = "Element Create";

        //    return resultViewModel;

        //}

        [HttpGet]
        public ResultViewModel Get(string id_user)
        {
            ResultViewModel resultViewModel = new ResultViewModel();
            var orders = db.OrderDetails.Where(i => i.UserId == id_user).ToList();
        
            if (orders == null)
            {
                resultViewModel.Success = false;
                resultViewModel.Message = "Element Not found";
                return resultViewModel;
            }
            else
            {
                resultViewModel.Success = true;
                resultViewModel.Message = "Element  found";
                resultViewModel.Data = orders;
                return resultViewModel;
            }

        }

    }
}
