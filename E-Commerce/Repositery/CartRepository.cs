using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repositery.InterfaceCategory;

namespace E_Commerce.Repositery
{
    public class CartRepository : Repositery<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}
