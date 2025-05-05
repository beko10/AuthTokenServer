using AuthTokenServer.CoreLayer.DataAccessLayer.Concrete;
using AuthTokenServer.DataAccessLayer.Abstract;
using AuthTokenServer.DataAccessLayer.Context;
using AuthTokenServer.EntityLayer.Entities;

namespace AuthTokenServer.DataAccessLayer.Concrete;

public class ProductRepositry : GenericRepository<Product, AppDbContext>, IProductRepository
{
    public ProductRepositry(AppDbContext context) : base(context)
    {
    }
}
