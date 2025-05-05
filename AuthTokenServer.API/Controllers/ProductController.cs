using AuthTokenServer.BusinessLayer.Abstract;
using AuthTokenServer.EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthTokenServer.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult ProductList()
    {
        var result = _productService.GetAllProducts();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(string id)
    {
        var result = _productService.GetProductById(id);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(AddProductDto product)
    {
        var result = await _productService.AddProductAsync(product);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductDto product)
    {
        var result = await _productService.UpdateProductAsync(product);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }
}