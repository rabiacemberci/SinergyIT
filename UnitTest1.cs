// Test Projesi - Mapper ve DbContext Mocklama

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests
{
    public class RepositoryTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly ApplicationDbContext _dbContext;

        public RepositoryTests()
        {
            _mapperMock = new Mock<IMapper>();

            // InMemory Database Kullanýmý
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Veritabanýný her test için temizle
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            // Test verilerini ekle
            _dbContext.Products.AddRange(new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 50 },
                new Product { Id = 2, Name = "Product 2", Price = 100 }
            });

            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduct()
        {
            // Arrange
            var repository = new Repository<Product>(_dbContext);
            var product = new Product { Id = 3, Name = "New Product", Price = 120 };

            // Act
            await repository.AddAsync(product);

            // Assert
            var addedProduct = await _dbContext.Products.FindAsync(3);
            Assert.NotNull(addedProduct);
            Assert.Equal("New Product", addedProduct.Name);
            Assert.Equal(120, addedProduct.Price);
        }

        [Fact]
        public void Mapper_ShouldMapSourceToDestination()
        {
            // Arrange
            var source = new Product { Id = 1, Name = "Source Product", Price = 50 };

            _mapperMock.Setup(m => m.Map<ProductDTO>(It.IsAny<Product>()))
                       .Returns(new ProductDTO { Id = source.Id, Name = source.Name, Price = source.Price });

            // Act
            var result = _mapperMock.Object.Map<ProductDTO>(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(source.Id, result.Id);
            Assert.Equal(source.Name, result.Name);
            Assert.Equal(source.Price, result.Price);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var repository = new Repository<Product>(_dbContext);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData(1, "Product 1", 50)]
        [InlineData(2, "Product 2", 100)]
        public async Task GetByIdAsync_ShouldReturnCorrectProduct(int id, string name, decimal price)
        {
            // Arrange
            var repository = new Repository<Product>(_dbContext);

            // Act
            var result = await repository.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(price, result.Price);
        }

        [Theory]
        [InlineData(1, 50)]
        [InlineData(2, 100)]
        public async Task DeleteAsync_ShouldRemoveProduct(int id, decimal price)
        {
            // Arrange
            var repository = new Repository<Product>(_dbContext);

            // Act
            await repository.DeleteAsync(id);

            // Assert
            var deletedProduct = await _dbContext.Products.FindAsync(id);
            Assert.Null(deletedProduct);
        }
    }
}

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
