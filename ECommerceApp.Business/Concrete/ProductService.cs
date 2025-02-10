﻿using ECommerce.Entities.Models;
using ECommerceApp.Business.Abstract;
using ECommerceApp.DataAccess.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Business.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IProductDal _productDal;

        public ProductService(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public Task AddAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _productDal.Get(p => p.ProductId == id);
            await _productDal.Delete(item);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _productDal.GetList();
        }

        public async Task<List<Product>> GetAllByCategoryId(int categoryId)
        {
            return await _productDal.GetList(p => p.CategoryId == categoryId || categoryId == 0);
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return _productDal.Get(p => p.ProductId == id);
        }

        public Task UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
