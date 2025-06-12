using Microsoft.AspNetCore.Mvc;
using TheProject.Application.DTOs;
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



    // NOVO ENDPOINT UPDATE
        [HttpPut("Update")]
        public async Task<ActionResult<Response<Products>>> Update([FromBody] UpdateProductRequest request)
        {
            var result = await _productsInterface.Update(
                request.Id,
                request.CategoryId,
                request.Name,
                request.UnitPrice,
                request.StockQuantity,
                request.Status
            );

            if (!result.Status)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }

