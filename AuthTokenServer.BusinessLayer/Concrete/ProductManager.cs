using AuthTokenServer.BusinessLayer.Abstract;
using AuthTokenServer.CoreLayer.DataAccessLayer.UnitOfWork;
using AuthTokenServer.CoreLayer.Utilities.Result;
using AuthTokenServer.DataAccessLayer.Abstract;
using AuthTokenServer.EntityLayer.DTOs;
using AuthTokenServer.EntityLayer.Entities;
using AutoMapper;

namespace AuthTokenServer.BusinessLayer.Concrete;

public class ProductManager : IProductService
{

    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public ProductManager(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> AddProductAsync(AddProductDto product)
    {
        var addedProduct = _mapper.Map<Product>(product);   
        await _productRepository.CreateAsync(addedProduct);
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult("Product added successfully");
    }

    public async Task<IResult> DeleteProductAsync(string productId)
    {
        var deletedProduct = await _productRepository.GetByIdAsync(productId);
        if (deletedProduct is null)
        {
            return new ErrorResult("Product not found");
        }
        _productRepository.Delete(deletedProduct);
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult("Product deleted successfully");
    }

    public IDataResult<IEnumerable<ProductDto>> GetAllProducts()
    {
        var productList =  _productRepository.GetAll(); 
        var productDtoList = _mapper.Map<IEnumerable<ProductDto>>(productList);
        if(!productDtoList.Any())
        {
            return new ErrorDataResult<IEnumerable<ProductDto>>(null, "Products not found");
        }
        return new SuccessDataResult<IEnumerable<ProductDto>>(productDtoList, "Products found");
    }

    public async Task<IResult> UpdateProductAsync(ProductDto product)
    {
        var hasProduct = await _productRepository.GetByIdAsync(product.Id);
        if (hasProduct is null)
        {
            return new ErrorResult("Product not found");
        }
        var updatedProduct = _mapper.Map<Product>(product);
        _productRepository.Update(updatedProduct); 
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult("Product updated successfully");

    }

    IDataResult<ProductDto> IProductService.GetProductById(string productId)
    {
       var hasProduct = _productRepository.GetByIdAsync(productId);
        if (hasProduct is null)
        {
            return new ErrorDataResult<ProductDto>(null, "Product not found");
        }
        var productDto = _mapper.Map<ProductDto>(hasProduct);
        return new SuccessDataResult<ProductDto>(productDto, "Product found");
    }
}
