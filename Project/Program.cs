// Ana Kod - CRUD Yapısı

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Data
{
    // Entity Sınıfı
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // DbContext Sınıfı
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        // EnsureCreated eklenmesi
        public static void InitializeDatabase(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }

    // IRepository Arayüzü
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }

    // Repository Sınıfı
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

    // Program Giriş Noktası
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TestDB;Trusted_Connection=True;",
                 sqlOptions => sqlOptions.EnableRetryOnFailure())
            .Options;


            using var context = new ApplicationDbContext(options);
            ApplicationDbContext.InitializeDatabase(context); // EnsureCreated ile tablo oluşturma

            var repository = new Repository<Product>(context);

            // Örnek CRUD İşlemleri
            Console.WriteLine("Ürün Ekleme İşlemi");
            var newProduct = new Product { Name = "Sample Product", Price = 89.99m };
            await repository.AddAsync(newProduct);
            Console.WriteLine("Ürün Eklendi: " + newProduct.Name);

            Console.WriteLine("\nTüm Ürünleri Listeleme");
            var products = await repository.GetAllAsync();
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}");
            }

            /* Console.WriteLine("\nÜrün Güncelleme");
            newProduct.Price = 77.99m;
            await repository.UpdateAsync(newProduct);
            Console.WriteLine("Ürün Güncellendi: " + newProduct.Name);

            Console.WriteLine("\nÜrün Silme");
            await repository.DeleteAsync(newProduct.Id);
            Console.WriteLine("Ürün Silindi: " + newProduct.Name);*/
        }
    }
}
