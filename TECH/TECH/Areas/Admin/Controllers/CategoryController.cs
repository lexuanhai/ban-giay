using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new CategoryModelView();
            if (id > 0)
            {
                model = _categoryService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetAll()
        {            
            var data = _categoryService.GetAll();
            return Json(new
            {
                Data = data
            });
        }

        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(CategoryModelView CategoryModelView)
        {

            bool isCategoryNameExist = false;
            if (CategoryModelView != null && !string.IsNullOrEmpty(CategoryModelView.name))
            {
                isCategoryNameExist = _categoryService.IsCategoryNameExist(CategoryModelView.name);               
            }

            if (!isCategoryNameExist)
            {
                _categoryService.Add(CategoryModelView);
                _categoryService.Save();
                return Json(new
                {
                    success = true
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    isCategoryNameExist = isCategoryNameExist
                });
            }

           

        }

        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _categoryService.UpdateStatus(id, status);
                _categoryService.Save();
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
        public JsonResult Update(CategoryModelView CategoryModelView)
        {
            bool isCategoryNameExist = false;
            if (CategoryModelView != null && !string.IsNullOrEmpty(CategoryModelView.name))
            {
                isCategoryNameExist = _categoryService.IsCategoryNameExist(CategoryModelView.name);
            }


            if (!isCategoryNameExist)
            {
                var result = _categoryService.Update(CategoryModelView);
                _categoryService.Save();
                return Json(new
                {
                    success = result
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    isCategoryNameExist = isCategoryNameExist
                });
            }


        }

     
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _categoryService.Deleted(id);
            _categoryService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(CategoryViewModelSearch categoryViewModelSearch)
        {
            var data = _categoryService.GetAllPaging(categoryViewModelSearch);
            return Json(new { data = data });
        }
    }
}
