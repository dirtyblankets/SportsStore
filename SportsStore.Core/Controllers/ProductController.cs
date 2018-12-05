using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        public ViewResult List(int productPage = 1)
            => View(new ProductsListViewModel
            {
                Products = productRepository.Products.OrderBy(p => p.ProductID)
                                            .Skip((productPage - 1) * PageSize)
                                            .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = productRepository.Products.Count()
                }
            });

		public int PageSize { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_productRepository"></param>
        public ProductController(IProductRepository _productRepository)
        {
            this.productRepository = _productRepository;
			this.PageSize = 4;
        }

        private IProductRepository productRepository;
    }
}