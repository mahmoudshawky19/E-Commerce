using E_Commerce.Data;
using E_Commerce.Models;

namespace E_Commerce.Repositery.InterfaceCategory
{
    public class ProductRepository : Repositery<Product>, IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
