﻿using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        //[CacheRemoveAspect("IProductService.Get")]
        [CacheRemoveAspect("Business.Abstract.IProductService.GetAll")]
        public IResult Add(Product product)
        {
            //business codes
            //validation
            //ValidationTool.Validate(new ProductValidator(), product);

            IResult result = BusinessRules.Run(CheckIfProductCountOfCategory(product.CategoryId),
                CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryNumberExceed());

            if (result !=null)
            {
                return result;
            }
            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);

            //Before BusinessRules Added

            /*if (CheckIfProductCountOfCategory(product.CategoryId).Success && CheckIfProductNameExists(product.ProductName).Success)
            {
                _productDal.Add(product);

                return new SuccessResult(Messages.ProductAdded);
            }
            */
        }

        [CacheAspect]//key,value
        [PerformanceAspect(1)]
        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);

        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int id)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == id));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetail()
        {
            //if (DateTime.Now.Hour == 19)
            //{
            //    return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            //}
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult();
        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted); 
        }

        private IResult CheckIfProductCountOfCategory(int categoryId)
        {
            //Select count(*) from products where categoryId=1
            var count = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (count >= 10)
            {
                return new ErrorResult(Messages.ProductCountExceed);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            //Another example -> _productDal.GetAll(p => p.ProductName == productName).Any();
            //Any() -> Determines whether a sequence contains any elements.
            var result = _productDal.GetAll(p => p.ProductName == productName).Count;
            if(result == 0)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.ProductNameAlreadyExists);
        }

        private IResult CheckIfCategoryNumberExceed()
        {
            var count = _categoryService.GetAll().Data.Count;
            if(count > 15)
            {
                return new ErrorResult(Messages.CategoryCountExceed);
            }
            return new SuccessResult();
        }

        public IResult AddTransactionalTest(Product product)
        {
            Add(product);
            if(product.UnitPrice < 10)
            {
                throw new Exception("");
            }
            Add(product);

            return null;
        }
    }
}
