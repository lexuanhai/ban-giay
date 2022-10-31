using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;

namespace TECH.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrdersService _ordersService;
        private readonly IAppUserService _appUserService;
        private readonly IProductsService _productsService;
        public OrdersController(IOrdersService ordersService,
            IAppUserService appUserService,
            IProductsService productsService)
        {
            _ordersService = ordersService;
            _appUserService = appUserService;
            _productsService = productsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OderDetail(int orderId)
        {
            var model = new OrdersModelView();
            if (orderId > 0)
            {
               
                model = _ordersService.GetByid(orderId);

                if (model != null && model.user_id.HasValue)
                {
                    model.totalstr = model.total.HasValue && model.total.Value > 0 ? model.total.Value.ToString("#,###") : "";

                    if (model.payment == 1)
                    {
                        model.paymentstr = "Ship Cod";
                    }
                    else if (model.payment == 2)
                    {
                        model.paymentstr = "VnPay";
                    }
                    else if (model.payment == 3)
                    {
                        model.paymentstr = "Momo";
                    }
                    else if (model.payment == 0)
                    {
                        model.paymentstr = "Mua trực tiếp";
                    }

                    var appuser = _appUserService.GetByid(model.user_id.Value);
                    if (appuser != null)
                    {
                        model.UserModelView = appuser;
                    }

                    var orderDetails = _ordersService.GetOrderDetails(model.id);
                    if (orderDetails != null && orderDetails.Count > 0)
                    {
                        foreach (var item in orderDetails)
                        {
                            if (item.product_id.HasValue && item.product_id.Value > 0)
                            {
                                var product = _productsService.GetByid(item.product_id.Value);
                                if (product != null)
                                {
                                    item.ProductModelView = product;
                                }
                            }
                           
                        }
                        model.OrdersDetailModelView = orderDetails;
                    }

                }

            }
            return View(model);
        }



        [HttpGet]
        public JsonResult GetOrderDetail(int orderId)
        {
            var model = new OrdersModelView();
            if (orderId > 0)
            {

                model = _ordersService.GetByid(orderId);

                if (model != null && model.user_id.HasValue)
                {
                    model.totalstr = model.total.HasValue && model.total.Value > 0 ? model.total.Value.ToString("#,###") : "";

                    if (model.payment == 1)
                    {
                        model.paymentstr = "Ship Cod";
                    }
                    else if (model.payment == 2)
                    {
                        model.paymentstr = "VnPay";
                    }
                    else if (model.payment == 0)
                    {
                        model.paymentstr = "Mua trực tiếp";
                    }

                    var appuser = _appUserService.GetByid(model.user_id.Value);
                    if (appuser != null)
                    {
                        model.UserModelView = appuser;
                    }

                    var orderDetails = _ordersService.GetOrderDetails(model.id);
                    if (orderDetails != null && orderDetails.Count > 0)
                    {
                        foreach (var item in orderDetails)
                        {
                            if (item.product_id.HasValue && item.product_id.Value > 0)
                            {
                                var product = _productsService.GetByid(item.product_id.Value);
                                if (product != null)
                                {
                                    item.ProductModelView = product;
                                }
                            }
                            item.pricestr = item.price.HasValue && item.price.Value > 0 ? item.price.Value.ToString("#,###") : "";

                        }
                        model.fee_shipstr = model.fee_ship.HasValue && model.fee_ship.Value > 0 ? model.fee_ship.Value.ToString("#,###") : "";
                        //model.totalOrderDetail = orderDetails.Sum(p => p.price*p.quantity);
                        model.totalOrderDetailStr = model.totalOrderDetail.HasValue && model.totalOrderDetail.Value > 0 ? model.totalOrderDetail.Value.ToString("#,###") : "";
                        model.OrdersDetailModelView = orderDetails;
                    }

                }
              
            }
            return Json(new
            {
                Data = model
            });
        }






        //[HttpGet]
        //public IActionResult AddView()
        //{
        //    return View();
        //}

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new OrdersModelView();
            if (id > 0)
            {
                model = _ordersService.GetByid(id);

               
                if (model != null && model.user_id.HasValue)
                {
                    var appuser = _appUserService.GetByid(model.user_id.Value);
                    model.customerStr = appuser.full_name;
                }

            }
            return Json(new
            {
                Data = model
            });
        }

        //[HttpGet]
        //public IActionResult AddOrUpdate()
        //{
        //    return View();
        //}

       
        [HttpPost]
        public JsonResult Add(OrdersModelView OrdersModelView)
        {
            var result = _ordersService.Add(OrdersModelView);
            _ordersService.Save();
            return Json(new
            {
                success = result
            });

        }

        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _ordersService.UpdateStatus(id, status);
                _ordersService.Save();
                return Json(new
                {
                    success = model
                });
            }
            return Json(new
            {
                success = false
            });

        }

        [HttpPost]
        public JsonResult Update(OrdersModelView OrdersModelView)
        {

           
            var result = _ordersService.Update(OrdersModelView);
            _ordersService.Save();
            return Json(new
            {
                success = result
            });

        }

      

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _ordersService.Deleted(id);
            _ordersService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(OrdersViewModelSearch ordersViewModelSearch)
        {
            if (ordersViewModelSearch != null && !string.IsNullOrEmpty(ordersViewModelSearch.name))
            {
                var _user = _appUserService.GetUserSearch(ordersViewModelSearch.name.Trim());
                if (_user != null && _user.Count > 0)
                {
                    ordersViewModelSearch.user_ids = _user.Select(u => u.id).ToList();
                }
            }

            var data = _ordersService.GetAllPaging(ordersViewModelSearch);

            foreach (var item in data.Results)
            {
                if (item != null && item.user_id.HasValue)
                {
                    var appuser = _appUserService.GetByid(item.user_id.Value);
                    item.customerStr = appuser.full_name;
                    if (item.payment == 1)
                    {
                        item.paymentstr = "Ship Cod";
                    }
                    else if (item.payment == 2)
                    {
                        item.paymentstr = "VnPay";
                    }
                    else if (item.payment == 0)
                    {
                        item.paymentstr = "Mua trực tiếp";
                    }
                    else if (item.payment == 3)
                    {
                        item.paymentstr = "Momo";
                    }
                }

            }
            return Json(new { data = data });
        }
    }
}
