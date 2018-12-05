using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
			//Arrange
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns((new Product[] {
					new Product { ProductID = 1, Name = "P1" },
					new Product { ProductID = 2, Name = "P2" },
					new Product { ProductID = 3, Name = "P3" },
					new Product { ProductID = 4, Name = "P4" },
					new Product { ProductID = 5, Name = "P5" },
				}).AsQueryable<Product>());

			ProductController productController = new ProductController(mock.Object);
			productController.PageSize = 3;

			//Act
			ProductsListViewModel result = productController.List(2).ViewData.Model as ProductsListViewModel;

			//Assert
			Product[] productArray = result.Products.ToArray();
			Assert.True(productArray.Length == 2);
			Assert.Equal("P4", productArray[0].Name);
			Assert.Equal("P5", productArray[1].Name);
		}
    }
}
