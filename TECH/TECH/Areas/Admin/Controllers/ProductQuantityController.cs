using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class ProductQuantityController : BaseController
    {
        private readonly IProductQuantityService _productQuantityService;
        public ProductQuantityController(IProductQuantityService productQuantityService)
        {
            _productQuantityService = productQuantityService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProductQuantityForProductId(int productId)
        {
            if (productId > 0)
            {
                var data = _productQuantityService.GetProductQuantity(productId);
                return Json(new
                {
                    Data = data,
                    Success = true
                });
            }
            return Json(new
            {                
                Success = false
            });

        }

        //[HttpGet]
        //public IActionResult AddOrUpdate()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult Add(CategoryModelView CategoryModelView)
        //{

        //    bool isCategoryNameExist = false;
        //    if (CategoryModelView != null && !string.IsNullOrEmpty(CategoryModelView.name))
        //    {
        //        isCategoryNameExist = _productQuantityService.IsCategoryNameExist(CategoryModelView.name);               
        //    }

        //    if (!isCategoryNameExist)
        //    {
        //        _productQuantityService.Add(CategoryModelView);
        //        _productQuantityService.Save();
        //        return Json(new
        //        {
        //            success = true
        //        });
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            isCategoryNameExist = isCategoryNameExist
        //        });
        //    }



        //}

        //[HttpGet]
        //public JsonResult UpdateStatus(int id,int status)
        //{
        //    if (id > 0)
        //    {
        //       var  model = _productQuantityService.UpdateStatus(id, status);
        //        _productQuantityService.Save();
        //        return Json(new
        //        {
        //            success = model
        //        });
        //    }
        //    return Json(new
        //    {
        //        success = false
        //    });

        //}

        //[HttpPost]
        //public JsonResult Update(CategoryModelView CategoryModelView)
        //{
        //    bool isCategoryNameExist = false;
        //    if (CategoryModelView != null && !string.IsNullOrEmpty(CategoryModelView.name))
        //    {
        //        isCategoryNameExist = _productQuantityService.IsCategoryNameExist(CategoryModelView.name);
        //    }


        //    if (!isCategoryNameExist)
        //    {
        //        var result = _productQuantityService.Update(CategoryModelView);
        //        _productQuantityService.Save();
        //        return Json(new
        //        {
        //            success = result
        //        });
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            isCategoryNameExist = isCategoryNameExist
        //        });
        //    }


        //}


        //[HttpPost]
        //public JsonResult Delete(int id)
        //{
        //    var result = _productQuantityService.Deleted(id);
        //    _productQuantityService.Save();
        //    return Json(new
        //    {
        //        success = result
        //    });
        //}

        //[HttpGet]
        //public JsonResult GetAllPaging(CategoryViewModelSearch categoryViewModelSearch)
        //{
        //    var data = _productQuantityService.GetAllPaging(categoryViewModelSearch);
        //    return Json(new { data = data });
        //}
    }
}
