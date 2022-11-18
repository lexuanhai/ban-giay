
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IProductQuantityService
    {           
        void Add(List<QuantityProductModelView> view);
        bool Update(List<QuantityProductModelView> view);
        bool Deleted(List<int> ids);
        List<QuantityProductModelView> GetProductQuantity(int productId);
        void Save();
    }

    public class ProductQuantityService : IProductQuantityService
    {
        private readonly IProductQuantityRepository _productQuantityRepository;
        private IUnitOfWork _unitOfWork;
        public ProductQuantityService(IProductQuantityRepository productQuantityRepository,
            IUnitOfWork unitOfWork)
        {
            _productQuantityRepository = productQuantityRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(List<QuantityProductModelView> view)
        {
            try
            {
                if (view != null && view.Count > 0)
                {
                    foreach (var item in view)
                    {
                        var image = new ProductQuantity
                        {
                            product_id = item.ProductId,
                            size_id = item.AppSizeId,
                            color_id = item.ColorId,
                            totalimport = item.TotalImported
                        };
                        _productQuantityRepository.Add(image);
                    }
                                
                }
            }
            catch (Exception ex)
            {
            }

        }
        public List<QuantityProductModelView> GetProductQuantity(int productId)
        {
            if (productId > 0)
            {
                var productQuantity = _productQuantityRepository.FindAll(p => p.product_id == productId).Select(p => new QuantityProductModelView
                {
                    Id = p.id,
                    ProductId = p.product_id,
                    ColorId = p.color_id,
                    AppSizeId = p.size_id,
                    TotalImported = p.totalimport,
                }).ToList();
                return productQuantity;
            }
            return null;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(List<QuantityProductModelView> view)
        {
            try
            {
                if (view != null && view.Count > 0)
                {
                    foreach (var item in view)
                    {
                        var dataServer = _productQuantityRepository.FindById(item.Id);
                        if (dataServer != null)
                        {
                            dataServer.product_id = item.ProductId;
                            dataServer.size_id = item.AppSizeId;
                            dataServer.color_id = item.ColorId;
                            dataServer.totalimport = item.TotalImported;
                            _productQuantityRepository.Update(dataServer);
                           
                        }
                    }
                    return true;
                }
               
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool Deleted(List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    foreach (var item in ids)
                    {
                        var dataServer = _productQuantityRepository.FindById(item);
                        if (dataServer != null)
                        {
                            _productQuantityRepository.Remove(dataServer);
                        }                        
                    }
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
       
    }
}
