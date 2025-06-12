using Microsoft.AspNetCore.Mvc;
using TheProject.Application.DTOs;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;
using TheProject.Infrastructure.Services.Product;
using TheProject.Application.DTOs;

namespace TheProject.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{

    
    private readonly IProductsInterface _productsInterface;
    public ProductsController(IProductsInterface productsInterface )
    {
        _productsInterface = productsInterface;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<Response<List<Products>>>> GetAll()
    {
        var products = await _productsInterface.GetAll();

        return Ok(products);
    }


    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] ProductUptadeDTO dto)
    {
       // dto.Id = id; 

        var result = await _productsInterface.Update(dto);
        if (!result.Status)
            return BadRequest(result.Message);

        return Ok(result);
    }

    }

