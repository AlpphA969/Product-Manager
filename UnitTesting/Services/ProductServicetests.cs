//using Application.Services;
//using AutoMapper;
//using Castle.DynamicProxy;
//using Domain.Entity;
//using Models.ViewModel;
//using Moq;
//using Persistence;
//using Persistence.Abstraction;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace Services
//{
//    public class ProductServicetests
//    {
//        [Fact]
//        public async Task GetProductById_WhenProductExists_ReturnsProduct()
//        {
//            // Arrange
//            var productId = Guid.NewGuid();

//            var product = new Product
//            {
//                Id = productId
//            };

//            var repositoryMock = new Mock<IProductRepository>();

//            repositoryMock
//                .Setup(x => x.FindByIdAsync(productId))
//                .ReturnsAsync(product);

//            var unitOfWorkMock = new Mock<IUnitOfWork>();

//            unitOfWorkMock
//                .Setup(x => x.ProductRepository)
//                .Returns(repositoryMock.Object);

//            var mapperMock = new Mock<IMapper>();
//            mapperMock.Setup(m => m.Map<ProductViewModel>(It.IsAny<Product>()))
//            .Returns(new ProductViewModel("Beige", "Roller Curtain") { Id = productId });

//            var service = new ProductService(mapperMock.Object , unitOfWorkMock.Object);

//            // Act
//            var result = await service.FindByIdAsync(productId);

//            // Assert
//            Assert.Equal(product.Id, result.Value.Id);
//        }
//        [Fact]
//        public async Task AddProductAsync_WithValidProduct_ReturnsProductViewModel()
//        {
//            // Arrange

//            var product = new Product
//            {
//                Color = "blue",
//                Name = "Test"
//            };

//            var productViewModel = new ProductViewModel
//            {
//                Id = product.Id,
//                Color = product.Color,
//                Name = product.Name
//            };

//            var repositoryMock = new Mock<IProductRepository>();

//            repositoryMock
//                .Setup(x => x.AddAsync(product))
//                .Returns(Task.CompletedTask);

//            var unitOfWorkMock = new Mock<IUnitOfWork>();

//            unitOfWorkMock
//                .Setup(x => x.ProductRepository)
//                .Returns(repositoryMock.Object);

//            var mapperMock = new Mock<IMapper>();

//            // ProductViewModel → Product
//            mapperMock
//                .Setup(x => x.Map<Product>(It.IsAny<ProductViewModel>()))
//                .Returns(product);

//            // Product → ProductViewModel
//            mapperMock
//                .Setup(x => x.Map<ProductViewModel>(It.IsAny<Product>()))
//                .Returns(productViewModel);

//            var service = new ProductService(
//                mapperMock.Object,
//                unitOfWorkMock.Object);


//            // Act

//            var result = await service.AddProductAsync(productViewModel);


//            // Assert

//            Assert.Equal(productViewModel, result);

//            repositoryMock.Verify(
//                x => x.AddAsync(product),
//                Times.Once);
//        }



//    }

//}
