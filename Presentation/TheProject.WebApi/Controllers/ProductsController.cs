using Microsoft.AspNetCore.Mvc;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;

namespace TheProject.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{


    private readonly IProductsInterface _productsInterface;
    public ProductsController(IProductsInterface productsInterface)
    {
        _productsInterface = productsInterface;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<Response<List<Products>>>> GetAll()
    {
        var products = await _productsInterface.GetAll();

        return Ok(products);
    }
}
