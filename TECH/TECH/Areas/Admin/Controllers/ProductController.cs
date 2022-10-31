using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;

namespace TECH.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductsService _productsService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductsService productsService,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
            ICategoryService categoryService)
        {
            _productsService = productsService;
            _hostingEnvironment = hostingEnvironment;
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddView()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new ProductModelView();
            if (id > 0)
            {
                model = _productsService.GetByid(id);

               
                if (model != null && !string.IsNullOrEmpty(model.name))
                {
                    if (model.category_id.HasValue && model.category_id.Value > 0)
                    {
                        var category = _categoryService.GetByid(model.category_id.Value);
                        model.categorystr = category.name;
                    }
                    else
                    {
                        model.categorystr = "";
                    }
                   
                }
                else
                {
                    model.categorystr = "Chờ xử lý";
                }

            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult UploadImageAvatar()
        //{
        //    var files = Request.Form.Files;
        //    if (files != null && files.Count > 0)
        //    {
        //        string folder = _hostingEnvironment.WebRootPath + $@"\avatar";

        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }
        //        var _lstImageName = new List<string>();

        //        foreach (var itemFile in files)
        //        {
        //            string filePath = Path.Combine(folder, itemFile.FileName);
        //            if (!System.IO.File.Exists(filePath))
        //            {
        //                _lstImageName.Add(itemFile.FileName);
        //                using (FileStream fs = System.IO.File.Create(filePath))
        //                {
        //                    itemFile.CopyTo(fs);
        //                    fs.Flush();
        //                }
        //            }
        //        }                
        //    }
        //    return Json(new
        //    {
        //        success = true
        //    });
        //}

        //[HttpPost]
        //public JsonResult IsEmailExist(string email)
        //{
        //    bool isMail = false;
        //    if (!string.IsNullOrEmpty(email))
        //    {
        //        isMail = _productsService.IsMailExist(email);
        //    }

        //    return Json(new
        //    {
        //        success = isMail
        //    });
        //}

        //[HttpPost]
        //public JsonResult IsPhoneExist(string phone)
        //{
        //    bool isphone = false;
        //    if (!string.IsNullOrEmpty(phone))
        //    {
        //        isphone = _productsService.IsPhoneExist(phone);
        //    }

        //    return Json(new
        //    {
        //        success = isphone
        //    }) ;
        //}

        [HttpPost]
        public IActionResult UploadImageProduct()
        {
            var files = Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                var imageFolder = $@"\product-image\";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var _lstImageName = new List<string>();

                foreach (var itemFile in files)
                {
                    string fileNameFormat = Regex.Replace(itemFile.FileName.ToLower(), @"\s+", "");
                    string filePath = Path.Combine(folder, fileNameFormat);
                    if (!System.IO.File.Exists(filePath))
                    {
                        _lstImageName.Add(fileNameFormat);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            itemFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
            return Json(new
            {
                success = true
            });
        }





        [HttpPost]
        public JsonResult Add(ProductModelView ProductModelView)
        {            
            bool isNameExist = false;
            if (ProductModelView != null && !string.IsNullOrEmpty(ProductModelView.name))
            {
                isNameExist = _productsService.IsProductNameExist(ProductModelView.name);                
            }


            if (!isNameExist)
            {
                var result = _productsService.Add(ProductModelView);
                _productsService.Save();
                return Json(new
                {
                    success = result
                });
            }
            return Json(new
            {
                success = false,
                isNameExist = isNameExist
            });

        }

        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _productsService.UpdateStatus(id, status);
                _productsService.Save();
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
        public JsonResult Update(ProductModelView ProductModelView)
        {

            //bool isNameExist = false;
            //if (ProductModelView != null && !string.IsNullOrEmpty(ProductModelView.name))
            //{
            //    isNameExist = _productsService.IsProductNameExist(ProductModelView.name);
            //}

            //if (!isNameExist)
            //{
            //    var result = _productsService.Update(ProductModelView);
            //    _productsService.Save();
            //    return Json(new
            //    {
            //        success = result
            //    });
            //}
            //return Json(new
            //{
            //    success = false,
            //    isNameExist = isNameExist
            //});

            var result = _productsService.Update(ProductModelView);
            _productsService.Save();
            return Json(new
            {
                success = result
            });

        }

        //[HttpPost]
        //public JsonResult AddUserRoles (int userId, int[] rolesId)
        //{
        //    try
        //    {
        //        _appUserRoleService.AddAppUserRole(userId, rolesId);

        //        return Json(new
        //        {
        //            success = true
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new
        //        {
        //            success = false
        //        });
        //    }

        //}

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _productsService.Deleted(id);
            _productsService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(ProductViewModelSearch colorViewModelSearch)
        {
            var data = _productsService.GetAllPaging(colorViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item.category_id.HasValue && item.category_id.Value > 0)
                {
                    var  category = _categoryService.GetByid(item.category_id.Value);
                    if (category != null && !string.IsNullOrEmpty(category.name))
                    {
                        item.categorystr = category.name;
                    }
                    else
                    {
                        item.categorystr = "Chờ xử lý";
                    }
                    
                }
                else
                {
                    item.categorystr = "";
                }

            }
            return Json(new { data = data });
        }


        [HttpGet]
        public JsonResult ProductSearch(ProductViewModelSearch productViewModelSearch)
        {
            productViewModelSearch.PageIndex = 1;
            productViewModelSearch.PageIndex = 20;
            var data = _productsService.GetAllPaging(productViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item.category_id.HasValue)
                {
                    var category = _categoryService.GetByid(item.category_id.Value);
                    if (category != null && !string.IsNullOrEmpty(category.name))
                    {
                        item.categorystr = category.name;
                    }
                    else
                    {
                        item.categorystr = "Chờ xử lý";
                    }

                }

            }
            return Json(new { data = data });
        }



        //[HttpGet]
        //public JsonResult GetAll()
        //{
        //    var data = _productsService.GetAll();
        //    return Json(new { Data = data });
        //}
    }
}
