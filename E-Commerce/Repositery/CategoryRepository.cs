using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repositery;
using E_Commerce.Repositery.InterfaceCategory;
 
namespace E_Commerce.Repository
{
    public class CategoryRepository : Repositery<Category>, ICategoryRepositery
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        // any additional logic
    }
}