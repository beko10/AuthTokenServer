using AuthTokenServer.CoreLayer.Utilities.Result;
using AuthTokenServer.EntityLayer.DTOs;

namespace AuthTokenServer.BusinessLayer.Abstract;

public interface IProductService
{
    Task<IResult> AddProductAsync(AddProductDto product);
    Task<IResult> UpdateProductAsync(ProductDto product);
    Task<IResult> DeleteProductAsync(string productId);
    IDataResult<ProductDto> GetProductById(string productId);
    IDataResult<IEnumerable<ProductDto>> GetAllProducts();
}
