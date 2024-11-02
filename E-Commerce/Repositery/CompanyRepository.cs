using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repositery.InterfaceCategory;

namespace E_Commerce.Repositery
{
    public class CompanyRepository : Repositery<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}
