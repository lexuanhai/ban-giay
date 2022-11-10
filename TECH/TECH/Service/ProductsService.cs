
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IProductsService
    {
        PagedResult<ProductModelView> GetAllPaging(ProductViewModelSearch ProductModelViewSearch);
        ProductModelView GetByid(int id);
        int Add(ProductModelView view);
        bool Update(ProductModelView view);
        bool Deleted(int id);
        void Save();
        bool IsProductNameExist(string name);
        bool UpdateStatus(int id, int status);
        int GetCount();
        int GetCountForCategory(int cateogryId);
        List<ProductModelView> GetProductReLated(int categoryId, int productId);
        List<ProductModelView> GetProductLike(int categoryId);
        List<ProductModelView> ProductSearch(string textSearch);
    }

    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private IUnitOfWork _unitOfWork;
        public ProductsService(IProductsRepository productsRepository,
            IUnitOfWork unitOfWork)
        {
            _productsRepository = productsRepository;
            _unitOfWork = unitOfWork;
        }

        

        public bool IsProductNameExist(string name)
        {
            var data = _productsRepository.FindAll().Any(p=>p.name == name);
            return data;
        }
        public ProductModelView GetByid(int id)
        {
            var data = _productsRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {                
                var model = new ProductModelView()
                {
                    id = data.id,
                    name = data.name,
                    category_id = data.category_id,
                    trademark = !string.IsNullOrEmpty(data.trademark) ? data.trademark : "",
                    price_sell = data.price_sell.Value,
                    price_import = data.price_import.Value,
                    price_reduced = data.price_reduced.Value,
                    //price_sell_str = data.price_sell.HasValue && data.price_sell.Value > 0 ? data.price_sell.Value.ToString("#,###") : "",
                    //price_import_str = data.price_import.HasValue && data.price_import.Value > 0 ? data.price_import.Value.ToString("#,###") : "",
                    //price_reduced_str = data.price_reduced.HasValue && data.price_reduced.Value > 0 ? data.price_reduced.Value.ToString("#,###") : "",
                    status = data.status,
                    description = data.description
                };
                return model;
            }
            return null;
        }
        public int Add(ProductModelView view)
        {
            try
            {
                if (view != null)
                {
                    var products = new Products
                    {
                        name = view.name,
                        //avatar = view.avatar,
                        category_id = view.category_id,
                        //slug = Regex.Replace(view.name.ToLower(), @"\s+", "-"),
                        //color = view.color,
                        price_sell = view.price_sell,
                        price_import = view.price_import,
                        price_reduced = view.price_reduced,
                        status = view.status,
                        trademark = view.trademark,
                        description = view.description,
                    };
                    _productsRepository.Add(products);
                    Save();
                    return products.id;                    
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;

        }
        public int GetCount()
        {
            int count = 0;
            count = _productsRepository.FindAll().Count();
            return count;
        }

        public int GetCountForCategory(int cateogryId)
        {
            int count = 0;
            count = _productsRepository.FindAll(p=>p.category_id == cateogryId).Count();
            return count;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(ProductModelView view)
        {
            try
            {
                var dataServer = _productsRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.category_id = view.category_id;
                    dataServer.name = view.name;
                    //dataServer.avatar = view.avatar;
                    //dataServer.slug = Regex.Replace(view.name.ToLower(), @"\s+", "-");
                    //dataServer.color = view.color;
                    //dataServer.quantity = view.quantity;
                    dataServer.description = view.description;
                    _productsRepository.Update(dataServer);                                        
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

      public  bool UpdateStatus(int id, int status)
        {
            try
            {
                var dataServer = _productsRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _productsRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _productsRepository.FindById(id);
                if (dataServer != null)
                {
                    _productsRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<ProductModelView> GetProductReLated(int categoryId, int productId)
        {
            var query = _productsRepository.FindAll(p=>p.status == 0).Where(p=>p.category_id == categoryId && p.id != productId);
            var data = query.Select(p => new ProductModelView()
            {
                id = p.id,
                category_id = p.category_id,
                name = p.name,
                //avatar = p.avatar,
                //slug = p.slug,
                //color = p.color,
                //price = p.price,
                ////quantity = p.quantity,
                //short_desc = p.short_desc,
                status = p.status,
                description = p.description
            }).Take(4).ToList();
            return data;
        }

        public List<ProductModelView> GetProductLike(int categoryId)
        {
            var query = _productsRepository.FindAll(p => p.status == 0).Where(p => p.category_id != categoryId);
            var data = query.Select(p => new ProductModelView()
            {
                id = p.id,
                category_id = p.category_id,
                name = p.name,
                //avatar = p.avatar,
                //slug = p.slug,
                //color = p.color,
                //price = p.price,
                ////quantity = p.quantity,
                //short_desc = p.short_desc,
                status = p.status,
                description = p.description,
            }).ToList();
            return data;
        }

        public PagedResult<ProductModelView> GetAllPaging(ProductViewModelSearch ProductModelViewSearch)
        {
            try
            {
                var query = _productsRepository.FindAll();

                if (ProductModelViewSearch.categoryId.HasValue && ProductModelViewSearch.categoryId.Value > 0)
                {
                    query = query.Where(c => c.category_id == ProductModelViewSearch.categoryId.Value);
                }
                
                if (!string.IsNullOrEmpty(ProductModelViewSearch.name))
                {
                    query = query.Where(c => c.name.ToLower().Trim().Contains(ProductModelViewSearch.name.ToLower().Trim()));
                }

                int totalRow = query.Count();
                query = query.Skip((ProductModelViewSearch.PageIndex - 1) * ProductModelViewSearch.PageSize).Take(ProductModelViewSearch.PageSize);
                var data = query.Select(p => new ProductModelView()
                {
                    id = p.id,
                    category_id = p.category_id,                    
                    name = p.name,
                    status = p.status,
                    trademark = p.trademark,
                    price_sell = p.price_sell,
                    price_import = p.price_import,
                    price_reduced = p.price_reduced,
                    description = p.description
                }).ToList();             
              
                var pagingData = new PagedResult<ProductModelView>
                {
                    Results = data,
                    CurrentPage = ProductModelViewSearch.PageIndex,
                    PageSize = ProductModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }       

        public List<ProductModelView> ProductSearch(string textSearch)
        {
            if (!string.IsNullOrEmpty(textSearch))
            {
                var query = _productsRepository.FindAll().Where(p=>p.name.ToLower().Contains(textSearch.ToLower().Trim())).Select(p => new ProductModelView()
                {
                    id = p.id,
                    category_id = p.category_id,
                    name = p.name,
                    //avatar = p.avatar,
                    //slug = p.slug,
                    //color = p.color,
                    //price = p.price,
                    ////quantity = p.quantity,
                    //short_desc = p.short_desc,
                    status = p.status,
                    description = p.description
                }).ToList();
                return query;
            }
            return null;
        }
    }
}
